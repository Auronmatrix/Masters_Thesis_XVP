namespace XVP.Infrastructure.Command.Interfaces
{
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;

    public interface IDocumentCommandService<TDocument>
    {
        Task<bool> DeleteAsync(string id);

        Task<Document> InsertAsync(TDocument data);
        
        Task<bool> ReplaceAsync(string id, TDocument item);
    }
}