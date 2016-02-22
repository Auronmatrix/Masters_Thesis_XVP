namespace XVP.Domain.Models.Models
{
    using System.Collections.Generic;

    using XVP.Infrastructure.Shared.Abstracts;
    using XVP.Infrastructure.Shared.CustomAttributes;

    [Document("Marketplace", "Carts", tenantSpecific: true)]
    public class CampaignCart : SerializableResource
    {
        public List<MediaItem> Items { get; set; }

        public double Total { get; set; }
    }
}
