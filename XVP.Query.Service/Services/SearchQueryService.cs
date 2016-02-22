namespace XVP.Infrastructure.Query.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AzureSearchClient;

    using XVP.Infrastructure.Query.Interfaces;
    using XVP.Infrastructure.Query.Repositories;
    using XVP.Infrastructure.Shared.Cache;

    public class SearchQueryService<TDocument> : ISearchQueryService<TDocument>
        where TDocument : Document
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
           return await this.repository.Lookup(indexName, key, fields);
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
