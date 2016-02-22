namespace XVP.Infrastructure.Command.Services
{
    using AzureSearchClient;

    using XVP.Infrastructure.Command.Interfaces;
    using XVP.Infrastructure.Command.Repositories;

    public class SearchCommandService<TDocument> : ISearchCommandService
        where TDocument : Document
    {
        private readonly ISeachCommandRepository<TDocument> repository;

        public SearchCommandService()
        {
            this.repository = new SeachCommandRepository<TDocument>();
        }
    }
}
