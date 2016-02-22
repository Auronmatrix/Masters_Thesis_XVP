namespace XVP.Domain.Models.Models
{
    using AzureSearchClient;

    using XVP.Infrastructure.Shared.Abstracts;
    using XVP.Infrastructure.Shared.CustomAttributes;

    [Document("DemoDB", "MediaItems")]
    public class MediaItem : SerializableResource
    {
        public string Name { get; set; }
        
        public double Price { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public GeographyPoint Location { get; set; }

        public string Type { get; set; }

        public bool IsAvailable { get; set; }

        public string Publisher { get; set; }
    }
}