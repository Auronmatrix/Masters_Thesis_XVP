namespace XVP.Infrastructure.Command.Repositories
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    using XVP.Infrastructure.Command.Interfaces;
    using XVP.Infrastructure.Shared.Abstracts;
    using XVP.Infrastructure.Shared.CustomAttributes;

    public class DocumentCommandRepository<TDocument> : IDocumentCommandRepository<TDocument>
        where TDocument : SerializableResource
    {
        private readonly string collectionId;
        private readonly string databaseId;
        private Database database;
        private DocumentCollection collection;
        private DocumentClient client;

        public DocumentCommandRepository(string tenantId = "")
        {
            var attribute = typeof(TDocument).GetCustomAttributes(typeof(DocumentAttribute), true).FirstOrDefault() as DocumentAttribute;
            if (attribute == null)
            {
                throw new ArgumentException("Model does not implement custom attribute 'Document'");
            }

            this.collectionId = attribute.CollectionId;
            var tenantDbId = string.Empty;
            if (attribute.TenantSpecific)
            {
                tenantDbId = tenantId;
            }
            this.databaseId = tenantDbId + attribute.DatabaseId;
        }

        public DocumentCommandRepository(string collectionId, string databaseId, string tenantId = "")
        {
            this.collectionId = collectionId; 
            this.databaseId = tenantId + databaseId;
        }

        private DocumentClient Client
        {
            get
            {
                if (this.client != null)
                {
                    return this.client;
                }

                var endpoint = ConfigurationManager.AppSettings["DocumentDB_Endpoint"];
                var authKey = ConfigurationManager.AppSettings["DocumentDB_AuthKey"];
                var endpointUri = new Uri(endpoint);
                this.client = new DocumentClient(endpointUri, authKey);
                return this.client;
            }
        }

        private DocumentCollection Collection
        {
            get
            {
                return this.collection ?? (this.collection = this.ReadOrCreateCollection(this.Database.SelfLink));
            }
        }

        private Database Database
        {
            get
            {
                return this.database ?? (this.database = this.ReadOrCreateDatabase());
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var doc = this.GetDocument(id);
                if (doc == null)
                {
                    return false;
                }

                await this.Client.DeleteDocumentAsync(doc.SelfLink);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Document deletion failed with exception message " + ex.Message);
            }
        }

        public async Task<Document> InsertAsync(TDocument data)
        {
            try
            {
                return await this.Client.CreateDocumentAsync(this.Collection.SelfLink, data);
            }
            catch (Exception ex)
            {
                throw new Exception("Document insertion failed with exception message " + ex.Message);
            }
        }
      
        public async Task<bool> ReplaceAsync(string id, TDocument item)
        {
            if (item == null || id != item.Id)
            {
                throw new Exception("Invalid arguments. Unable to replace document with ID null or different to document being replaced");
            }

            try
            {
                var doc = this.GetDocument(id);

                if (doc == null)
                {
                    return false;
                }

                await this.Client.ReplaceDocumentAsync(doc.SelfLink, item);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Document update failed with exception message " + ex.Message);
            }
        }

        private Document GetDocument(string id)
        {
            return this.Client.CreateDocumentQuery<Document>(this.Collection.DocumentsLink)
                        .Where(d => d.Id == id)
                        .AsEnumerable()
                        .FirstOrDefault();
        }

        private DocumentCollection ReadOrCreateCollection(string databaseLink)
        {
            var col = this.Client.CreateDocumentCollectionQuery(databaseLink)
                          .Where(c => c.Id == this.collectionId)
                          .AsEnumerable()
                          .FirstOrDefault()
                      ?? this.Client.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection { Id = this.collectionId }).Result;
            return col;
        }

        private DocumentCollection ReadOrCreateCollection(string databaseLink, string collectionName)
        {
            var col = this.Client.CreateDocumentCollectionQuery(databaseLink)
                          .Where(c => c.Id == collectionName)
                          .AsEnumerable()
                          .FirstOrDefault()
                      ?? this.Client.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection { Id = collectionName }).Result;
            return col;
        }

        private Database ReadOrCreateDatabase()
        {
            var db = this.Client.CreateDatabaseQuery()
                         .Where(d => d.Id == this.databaseId)
                         .AsEnumerable()
                         .FirstOrDefault()
                     ?? this.Client.CreateDatabaseAsync(new Database { Id = this.databaseId }).Result;
            return db;
        }
    }
}