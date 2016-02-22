namespace XVP.Infrastructure.Command.Services
{
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;

    using XVP.Infrastructure.Command.Interfaces;
    using XVP.Infrastructure.Command.Repositories;
    using XVP.Infrastructure.Shared.Abstracts;
    using XVP.Infrastructure.Shared.Cache;

    public class DocumentCommandService<TDocument> : IDocumentCommandService<TDocument>
        where TDocument : SerializableResource
    {
        private readonly DocumentCommandRepository<TDocument> repository;

        public DocumentCommandService(string currentTenantId = "")
        {
            this.repository = new DocumentCommandRepository<TDocument>(currentTenantId);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            await CacheService.Cache.DeleteIfExists(id);
            return await this.repository.DeleteAsync(id);
        }

        public async Task<Document> InsertAsync(TDocument data)
        {
            var result = await this.repository.InsertAsync(data);
            await CacheService.Cache.DeleteIfExists(result.Id);
            return result;
        }
        
        public async Task<bool> ReplaceAsync(string id, TDocument item)
        {
            await CacheService.Cache.DeleteIfExists(id);
            return await this.repository.ReplaceAsync(id, item);
        }
    }
}