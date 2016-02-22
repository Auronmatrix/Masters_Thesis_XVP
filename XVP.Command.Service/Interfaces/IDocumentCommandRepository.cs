namespace XVP.Infrastructure.Command.Interfaces
{
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;

    using XVP.Infrastructure.Shared.Abstracts;

    public interface IDocumentCommandRepository<TDocument>
        where TDocument : SerializableResource
    {
        Task<bool> DeleteAsync(string id);

        Task<Document> InsertAsync(TDocument data);
     
        Task<bool> ReplaceAsync(string id, TDocument item);
    }
}