namespace XVP.App_Start
{
    using System.Collections.Generic;
    using XVP.Domain.DataObjects;
    using XVP.Service.Services;

    public static class TenantManager
    {
        private static readonly List<Tenant> DefaultTenants = new List<Tenant>
                                                         {
                                                             new Tenant
                                                                 {
                                                                     TenantId = 1,
                                                                     Name = "ao",
                                                                     FolderName = "tenant-ao",
                                                                     HostName = "ao.dev.fithesis.info"
                                                                 },
                                                             new Tenant
                                                                 {
                                                                     TenantId = 2,
                                                                     Name = "xv",
                                                                     FolderName = "tenant-xv",
                                                                     HostName = "xv.dev.fithesis.info"
                                                                 }
                                                         };

        private static List<Tenant> tenants;

        public static List<Tenant> Tenants
        {
            get
            {
                if (tenants == null)
                {
                    PopulateTenants();
                    if (tenants == null)
                    {
                        tenants = DefaultTenants;
                    }
                }
                return tenants;
            }

            set
            {
                tenants = value;
            }
        }

        public static async void PopulateTenants()
        {
            var documentService = new DocumentService<Tenant>();
            tenants = documentService.GetAll();
            if (Tenants == null)
            {
                foreach (var defaultTenant in DefaultTenants)
                {
                    await documentService.InsertAsync(defaultTenant);
                }
            }

            tenants = documentService.GetAll();
        }
    }
}
