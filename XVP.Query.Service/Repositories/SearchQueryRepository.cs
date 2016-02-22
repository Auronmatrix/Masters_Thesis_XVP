namespace XVP.Infrastructure.Query.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    using AzureSearchClient;

    using XVP.Infrastructure.Query.Interfaces;
    using XVP.Infrastructure.Shared.Logging;

    public class SearchQueryRepository<TDocument> : ISeachQueryRepository<TDocument>
        where TDocument : Document
    {
        private readonly string serviceName;

       private readonly string queryKey;

        private readonly string managementKey;

       private Service azureSearchService;

       public SearchQueryRepository()
       {
           this.serviceName = ConfigurationManager.AppSettings["AzureSearchServiceName"];
           this.queryKey = ConfigurationManager.AppSettings["AzureSearchQueryKey"];
           this.managementKey = ConfigurationManager.AppSettings["AzureSearchManagementKey"];
       }
     
       private Service AzureSearchService
       {
           get
           {
               return this.azureSearchService
                      ?? (this.azureSearchService = new Service(this.serviceName, this.queryKey, this.managementKey));
           }
       }

        public async Task LoadIndexesAsync()
        {
            await this.AzureSearchService.LoadIndexesAsync();
        }

        public async Task<QueryReply<TDocument>> QueryAsync(string indexName, QueryParameters parameters)
        {
            QueryReply<TDocument> result = null;
            if (this.GetIndexByName(indexName) != null)
            {
                result = await this.AzureSearchService.Indexes[indexName].QueryAsync<TDocument>(parameters);
            }
            else
            {
                ElmahLogger.LogError(new Exception("QueryAsync against non existent index"));
            }

            return result;
        }

        public async Task<TDocument> Lookup(string indexName, string key, IEnumerable<string> fields = null)
        {
            TDocument result = null;
            if (this.GetIndexByName(indexName) != null)
            {
                result = await this.AzureSearchService.Indexes[indexName].Lookup<TDocument>(key, fields);
            }
            else
            {
            ElmahLogger.LogError(new Exception("Lookup against non existent index"));
            }

            return result;
        }

       public async Task<SuggestionReply<TSuggestion>> Suggest<TSuggestion>(string indexName, TSuggestion suggest, SuggestionParameters parameters) where TSuggestion : Suggestion
        {
            SuggestionReply<TSuggestion> result = null;
            if (this.GetIndexByName(indexName) != null)
            {
                result = await this.AzureSearchService.Indexes[indexName].SuggestAsync<TSuggestion>(parameters);
            }
            else
            {
                ElmahLogger.LogError(new Exception("QueryAsync against non existent index"));
            }

            return result; 
        }
       
     public Index GetIndexByName(string name)
       {
           Index result = null;
           try
           {
              result = this.AzureSearchService.Indexes.FirstOrDefault(x => x.Name == name);
           }
           catch (AzureSearchException ex)
           {
               if (ex.InnerException != null)
               {
                     ElmahLogger.LogError(ex.InnerException);
               }

                 ElmahLogger.LogError(ex);
           }
           catch (Exception ex)
           {
                 ElmahLogger.LogError(ex);
           }

           return result;
       }

       public async Task<IndexStatisticsReply> GetIndexStatistics(string indexName)
       {
           IndexStatisticsReply result = null;
           try
           {
               result = await this.AzureSearchService.Indexes[indexName].GetStatisticsAsync();
           }
           catch (AzureSearchException ex)
           {
               if (ex.InnerException != null)
               {
                     ElmahLogger.LogError(ex.InnerException);
               }

                ElmahLogger.LogError(ex);
           }
           catch (Exception ex)
           {
                 ElmahLogger.LogError(ex);
           }

           return result;
       }
       
      public async Task<IEnumerable<Index>> GetIndexes()
       {
           return await this.AzureSearchService.Indexes.ListIndexesAsync();
       }
    }
}
