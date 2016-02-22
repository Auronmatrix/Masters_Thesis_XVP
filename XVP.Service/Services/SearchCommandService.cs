namespace XVP.Service.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AzureSearchClient;

    using XVP.Repositories.Repositories;

    public class SearchCommandService<TDocument> where TDocument : Document
    {
        private readonly SeachCommandRepository<TDocument> repository;

        public SearchCommandService()
        {
            this.repository = new SeachCommandRepository<TDocument>();
        }
    }
}
