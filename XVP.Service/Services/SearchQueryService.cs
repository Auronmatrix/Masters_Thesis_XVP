namespace XVP.Service.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AzureSearchClient;

    using XVP.Repositories.Repositories;

    public class SearchQueryService<TDocument> where TDocument : Document
    {
        private readonly SearchQueryRepository<TDocument> repository;

        public SearchQueryService()
        {
            this.repository = new SearchQueryRepository<TDocument>();
        }

        public async Task LoadIndexesAsync()
        {
            await this.repository.LoadIndexesAsync();
        }

        public async Task<QueryReply<TDocument>> QueryAsync(string indexName, QueryParameters parameters)
        {
            return await this.repository.QueryAsync(indexName, parameters);
        }

        public async Task<TDocument> Lookup(string indexName, string key, IEnumerable<string> fields = null)
        {
            var result = CacheService.Cache.Get<TDocument>(key);
            if (result != null)
            {
                return result;
            }

            result = await this.repository.Lookup(indexName, key, fields);
            CacheService.Cache.SetAsync(key, result);
            return result;
        }

        public async Task<SuggestionReply<TSuggestion>> Suggest<TSuggestion>(string indexName, TSuggestion suggest, SuggestionParameters parameters) where TSuggestion : Suggestion
        {
            return await this.repository.Suggest(indexName, suggest, parameters);
        }

       public async Task<IEnumerable<Index>> GetIndexes()
       {
           return await this.repository.GetIndexes();
       }
    }
}
