namespace XVP.Infrastructure.Query.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using XVP.Infrastructure.Shared.Abstracts;

    public interface IDocumentQueryRepository<TDocument> 
        where TDocument : SerializableResource
    {
        SingleResult<TDocument> Lookup(string id);

        List<TDocument> GetAll();

        TDocument GetById(string id);

        IQueryable<TDocument> Query();
    }
}