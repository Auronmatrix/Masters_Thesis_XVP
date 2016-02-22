namespace XVP.Domain.Models.ViewModels
{
    using AzureSearchClient;

    using XVP.Infrastructure.Shared.Abstracts;

    public class MediaItemViewModel : SerializableResource
    {
        public string Name { get; set; }
        
        public string Publisher { get; set; }

        public double Price { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public GeographyPoint Location { get; set; }

        public string Type { get; set; }

        public bool IsAvailable { get; set; }
    }
}