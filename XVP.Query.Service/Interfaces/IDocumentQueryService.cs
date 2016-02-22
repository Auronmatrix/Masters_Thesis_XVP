namespace XVP.Infrastructure.Query.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    public interface IDocumentQueryService<TDocument>
    {
        SingleResult<TDocument> Lookup(string id);

        List<TDocument> GetAll();

        TDocument GetById(string id);

        IQueryable<TDocument> Query();
    }
}