namespace XVP.Service.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Caching;
    using System.Web.Http;

    using Microsoft.Azure.Documents;

    using Newtonsoft.Json;

    using StackExchange.Redis;

    using XVP.Domain.DataObjects;
    using XVP.Repositories.Repositories;

    public interface IDocumentCommandService<TDocument>
    {
        Task<bool> DeleteAsync(string id);

        Task<Document> InsertAsync(TDocument data);
        
        Task<bool> ReplaceAsync(string id, TDocument item);
    }

    public class DocumentCommandService<TDocument> : IDocumentCommandService<TDocument>
        where TDocument : SerializableResource
    {
        private const string SetCacheKey = "DocumentService.GetAll()";

        private readonly DocumentCommandRepository<TDocument> repository;

        public DocumentCommandService(string currentTenantId = "")
        {
            this.repository = new DocumentCommandRepository<TDocument>(currentTenantId);
        }

        public Task<bool> DeleteAsync(string id)
        {
            CacheService.Cache.KeyDelete(id);
            CacheService.Cache.KeyDelete(SetCacheKey);
            return this.repository.DeleteAsync(id);
        }

        public async Task<Document> InsertAsync(TDocument data)
        {
            var result = await this.repository.InsertAsync(data);
            CacheService.Cache.SetToJsonAsync(result.Id, data);
            await CacheService.Cache.ListRightPushAsync(SetCacheKey, CacheService.Cache.StringGet(result.Id));
            return result;
        }
        
        public Task<bool> ReplaceAsync(string id, TDocument item)
        {
            CacheService.Cache.SetToJsonAsync(id, item);
            CacheService.Cache.KeyDelete(SetCacheKey);
            return this.repository.ReplaceAsync(id, item);
        }
    }
}