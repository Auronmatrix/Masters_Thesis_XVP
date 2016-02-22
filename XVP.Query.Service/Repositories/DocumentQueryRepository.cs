namespace XVP.Infrastructure.Query.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Http;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    using XVP.Infrastructure.Query.Interfaces;
    using XVP.Infrastructure.Shared.Abstracts;
    using XVP.Infrastructure.Shared.CustomAttributes;

    public class DocumentQueryRepository<TDocument> : IDocumentQueryRepository<TDocument>
        where TDocument : SerializableResource
    {
        private readonly string collectionId;
        private readonly string databaseId;
        private Database database;
        private DocumentCollection collection;
        private DocumentClient client;

        public DocumentQueryRepository(string tenantId = "")
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

        public DocumentQueryRepository(string collectionId, string databaseId, string tenantId = "")
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

        public SingleResult<TDocument> Lookup(string id)
        {
            try
            {
                return SingleResult.Create<TDocument>(
                    this.Client.CreateDocumentQuery<TDocument>(this.Collection.DocumentsLink)
                    .Where(d => d.Id == id)
                    .Select<TDocument, TDocument>(d => d));
            }
            catch (Exception)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public List<TDocument> GetAll()
        {
              return this.Client.CreateDocumentQuery<TDocument>(this.Collection.DocumentsLink).ToList();
        }

        public TDocument GetById(string id)
        {
            try
            {
                return this.Client.CreateDocumentQuery<TDocument>(this.Collection.DocumentsLink).Where(x => x.Id == id).AsEnumerable().FirstOrDefault();
            }
            catch (Exception)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public IQueryable<TDocument> Query()
        {
            try
            {
                return this.Client.CreateDocumentQuery<TDocument>(this.Collection.DocumentsLink);
            }
            catch (Exception)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
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