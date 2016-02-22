namespace XVP.Infrastructure.Command.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AzureSearchClient;

    public interface ISeachCommandRepository<TDocument>
        where TDocument : Document
    {
        Task<bool> CreateIndex(Index index);

        Task<bool> DeleteIndex(Index index);

        Task<bool> DeleteIndex(string indexName);

        Task<bool> CreateIndexForSearchableService(TDocument model, string indexName);

        Task<bool> CreateDocumentsInIndex(List<TDocument> documents, string indexName);

        Task<bool> CreateDocumentInIndex(TDocument document, string indexName);

    }
}