namespace XVP.Domain.Commands.Commands
{
    using System.Linq;

    using Newtonsoft.Json;

    using XVP.Domain.Models.Models;
    using XVP.Infrastructure.Command.Interfaces;
    using XVP.Infrastructure.Command.Services;

    public class CreateCampaignCartCommand : ICommand
    {
        public CreateCampaignCartCommand(string tenant, CampaignCart cart)
        {
            this.Tenant = tenant;
            this.Cart = cart;
        }

        [JsonProperty]
        public CampaignCart Cart { get; private set; }

        [JsonProperty]
        public string Tenant { get; private set; }

        public void Execute()
        {
            IDocumentCommandService<CampaignCart> service = new DocumentCommandService<CampaignCart>();
            var total = this.Cart.Items.Sum(x => x.Price);
            this.Cart.Total = total;
            service.InsertAsync(this.Cart);
        }
    }
}
