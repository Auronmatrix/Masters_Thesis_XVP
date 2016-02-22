namespace XVP.Domain.Models.ViewModels
{
    using System.Collections.Generic;

    using XVP.Infrastructure.Shared.Abstracts;

    public class CampaignCartViewModel : SerializableResource
    {
        public List<MediaItemViewModel> Items { get; set; }

        public double Total { get; set; }
    }
}
