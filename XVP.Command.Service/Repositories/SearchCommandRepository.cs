namespace XVP.Infrastructure.Command.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    using AzureSearchClient;

    using XVP.Infrastructure.Command.Interfaces;

    public class SeachCommandRepository<TDocument> : ISeachCommandRepository<TDocument>
        where TDocument : Document
    {
       private readonly string serviceName;

       private readonly string queryKey;

       private readonly string managementKey;

       private Service azureSearchService;

       public SeachCommandRepository()
       {
           this.serviceName = ConfigurationManager.AppSettings["AzureSearchServiceName"];
           this.queryKey = ConfigurationManager.AppSettings["AzureSearchQueryKey"];
           this.managementKey = ConfigurationManager.AppSettings["AzureSearchManagementKey"];
       }
     
       public Service AzureSearchService
       {
           get
           {
               return this.azureSearchService
                      ?? (this.azureSearchService = new Service(this.serviceName, this.queryKey, this.managementKey));
           }
       }
        
      public async Task<bool> CreateIndex(Index index)
       {
           var result = false;
           try
           {
               if (this.GetIndexByName(index.Name) == null)
               {
                   result = await this.AzureSearchService.Indexes.CreateIndexAsync(index);
               }
           }
           catch (Exception ex)
           {
               if (ex.InnerException != null)
               {
                     throw new Exception("Index creation failed with exception message " + ex.Message + " and inner exception " + ex.InnerException.Message);
               }

               throw new Exception("Index creation failed with exception message " + ex.Message);
           }
           
           return result;
       }

       public async Task<bool> DeleteIndex(Index index)
       {
           var result = false;
           try
           {
              result = await this.AzureSearchService.Indexes.DeleteIndexAsync(index);
           }
           catch (Exception ex)
           {
               if (ex.InnerException != null)
               {
                   throw new Exception("Index deletion failed with exception message " + ex.Message + " and inner exception " + ex.InnerException.Message);
               }

               throw new Exception("Index deletion failed with exception message " + ex.Message);
           }

           return result;
       }

       public async Task<bool> DeleteIndex(string indexName)
       {
           var result = false;
           try
           {
               if (this.GetIndexByName(indexName) != null)
               {
                   result = await this.AzureSearchService.Indexes.DeleteIndexAsync(indexName);
               }
              
           }
           catch (Exception ex)
           {
               if (ex.InnerException != null)
               {
                   throw new Exception("Index deletion failed with exception message " + ex.Message + " and inner exception " + ex.InnerException.Message);
               }

               throw new Exception("Index deletion failed with exception message " + ex.Message);
           }
           return result;
       }

       public async Task<bool> CreateIndexForSearchableService(TDocument model, string indexName)
       {
           var result = false;
           try
           {
               var fields = model.GetFields();
               var index = new Index(fields, indexName);
               result = await this.CreateIndex(index);
           }
           catch (Exception ex)
           {
               if (ex.InnerException != null)
               {
                   throw new Exception("Index creation failed with exception message " + ex.Message + " and inner exception " + ex.InnerException.Message);
               }

               throw new Exception("Index creation failed with exception message " + ex.Message);
           }

           if (!result)
           {
               throw new Exception("Index Creation Failed. Result returned false");
           }

           return true;
       }

       public async Task<bool> CreateDocumentsInIndex(List<TDocument> documents, string indexName)
       {
           var result = false;
           try
           {
               if (this.GetIndexByName(indexName) == null)
               {
                   result = await this.CreateIndexForSearchableService(documents.First(), indexName);
               }

               result = await this.AzureSearchService.Indexes[indexName].CreateDocumentsAsync(documents);
           }
           catch (Exception ex)
           {
               if (ex.InnerException != null)
               {
                   throw new Exception("Document in index creation failed with exception message " + ex.Message + " and inner exception " + ex.InnerException.Message);
               }

               throw new Exception("Document in index creation failed with exception message " + ex.Message);
           }

           return result;
       }

       public async Task<bool> CreateDocumentInIndex(TDocument document, string indexName)
       {
           var result = false;
           try
           {
               if (this.GetIndexByName(indexName) == null)
               {
                   result = await this.CreateIndexForSearchableService(document, indexName);
               }

               result = await this.AzureSearchService.Indexes[indexName].CreateDocumentAsync(document);
           }
           catch (Exception ex)
           {
               if (ex.InnerException != null)
               {
                   throw new Exception("Document in index creation failed with exception message " + ex.Message + " and inner exception " + ex.InnerException.Message);
               }

               throw new Exception("Document in index creation failed with exception message " + ex.Message);
           }

           return result;
       }

      private Index GetIndexByName(string name)
       {
           Index result = null;
           try
           {
               result = this.AzureSearchService.Indexes.FirstOrDefault(x => x.Name == name);
           }
           catch (Exception ex)
           {
               if (ex.InnerException != null)
               {
                   throw new Exception("Index retrieval failed with exception message " + ex.Message + " and inner exception " + ex.InnerException.Message);
               }

               throw new Exception("Index retrieval failed with exception message " + ex.Message);
           }

           return result;
       }
    }
}
