namespace XVP.Infrastructure.Query.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using XVP.Infrastructure.Query.Interfaces;
    using XVP.Infrastructure.Query.Repositories;
    using XVP.Infrastructure.Shared.Abstracts;
    using XVP.Infrastructure.Shared.Cache;

    public class DocumentQueryService<TDocument> : IDocumentQueryService<TDocument>
        where TDocument : SerializableResource
    {
        private const string SetCacheKey = "DocumentService.GetAll()";

        private readonly IDocumentQueryRepository<TDocument> repository;

        public DocumentQueryService(string currentTenantId = "")
        {
            this.repository = new DocumentQueryRepository<TDocument>(currentTenantId);
        }

        public SingleResult<TDocument> Lookup(string id)
        {
           return this.repository.Lookup(id);
        }

        public List<TDocument> GetAll()
        {
            var result = this.repository.GetAll();
            if (result != null)
            {
                CacheService.Cache.SetList(SetCacheKey, result);
            }
            
            return result;
        }

        public TDocument GetById(string id)
        {
            var result = (TDocument)CacheService.Cache.GetFromJson<TDocument>(id);
            if (result != null)
            {
                return result;
            }

            result = this.repository.GetById(id);
            if (result != null)
            {
                CacheService.Cache.SetToJson(result.Id, result);
            }
           
            return result;
        }

        public IQueryable<TDocument> Query()
        {
            return this.repository.Query();
        }
    }
}