namespace XVP.Domain.Models.ViewModels
{
    using XVP.Infrastructure.Shared.Abstracts;

    public class TenantViewModel : SerializableResource
    {
        public int TenantId { get; set; }

        public string Name { get; set; }

        public string FolderName { get; set; }

        public string HostName { get; set; }
    }
}
