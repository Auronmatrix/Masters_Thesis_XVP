namespace XVP.Infrastructure.Query.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AzureSearchClient;

    public interface ISeachQueryRepository<TDocument>
        where TDocument : Document
    {
        Task LoadIndexesAsync();

        Task<QueryReply<TDocument>> QueryAsync(string indexName, QueryParameters parameters);

        Task<TDocument> Lookup(string indexName, string key, IEnumerable<string> fields = null);

        Task<SuggestionReply<TSuggestion>> Suggest<TSuggestion>(string indexName, TSuggestion suggest, SuggestionParameters parameters) where TSuggestion : Suggestion;

        Index GetIndexByName(string name);

        Task<IndexStatisticsReply> GetIndexStatistics(string indexName);

        Task<IEnumerable<Index>> GetIndexes();
    }
}