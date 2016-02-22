namespace XVP.Service.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using XVP.Domain.DataObjects;
    using XVP.Repositories.Repositories;

    public interface IDocumentQueryService<TDocument>
    {
        SingleResult<TDocument> Lookup(string id);

        List<TDocument> GetAll();

        TDocument GetById(string id);

        IQueryable<TDocument> Query();
    }

    public class DocumentQueryService<TDocument> : IDocumentQueryService<TDocument>
        where TDocument : SerializableResource
    {
        private const string SetCacheKey = "DocumentService.GetAll()";

        private readonly DocumentQueryRepository<TDocument> repository;

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
            var result = CacheService.Cache.GetFromJson<TDocument>(id) as TDocument;
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