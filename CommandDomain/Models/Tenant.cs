namespace XVP.Domain.Models.Models
{
    using XVP.Infrastructure.Shared.Abstracts;
    using XVP.Infrastructure.Shared.CustomAttributes;

    [Document("CoreDB", "Tenants", tenantSpecific: false)]
    public class Tenant : SerializableResource
    {
        public int TenantId { get; set; }

        public string Name { get; set; }

        public string FolderName { get; set; }

        public string HostName { get; set; }
    }
}
