namespace XVP.Domain.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using XVP.Domain.Models.ViewModels;
    using XVP.Infrastructure.Shared.Abstracts;
    using XVP.Infrastructure.Shared.CustomAttributes;

    [Document("Marketplace", "Campaigns", tenantSpecific: true)]
    public class Campaign : SerializableResource
    {
        public List<MediaItem> MediaItems { get; set; }

        public List<Motif> Motifs { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string Name { get; set; }

        public Guid User { get; set; }
        

    }
}
