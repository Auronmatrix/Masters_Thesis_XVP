namespace XVP.Domain.Commands.Commands
{
    using Newtonsoft.Json;

    using XVP.Infrastructure.Command.Interfaces;
    using XVP.Infrastructure.Command.Services;
    using XVP.Infrastructure.Shared.Abstracts;

    public class DeleteGenericObjectCommand<T> : ICommand where T : SerializableResource
    {
        public DeleteGenericObjectCommand(string tenant, string itemId)
        {
            this.ItemId = itemId;
            this.Tenant = tenant;
        }

         [JsonProperty]
        public string Tenant { get; private set; }

         [JsonProperty]
        public string ItemId { get; private set; }
         

        public void Execute()
        {
            IDocumentCommandService<T> service = new DocumentCommandService<T>(this.Tenant);
            var result = service.DeleteAsync(this.ItemId).Result;
        }
    }
}
