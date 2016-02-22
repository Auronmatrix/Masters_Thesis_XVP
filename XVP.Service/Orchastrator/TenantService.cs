namespace XVP.Service.Orchastrator
{
    using System.Collections.Generic;
    using System.Linq;

    using XVP.Domain.DataObjects;

    public static class TenantService
    {
        public static List<Tenant> Tenants { get; set; }

        public static List<Tenant> FetchDefault()
        {
            return new List<Tenant>
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
        }

        public static void SetToDefault()
        {
            Tenants = FetchDefault();
        }

        public static Tenant GetCurrentTenant(string host)
        {
            if (Tenants == null || !Tenants.Any())
            {
                SetToDefault();
            }

            if (host == null)
            {
                return Tenants[0];
            }

            var tenant = Tenants.Where(p =>
            {
                var match = p.Name + ".";
                return host.StartsWith(match);
            }).FirstOrDefault() ?? TenantService.Tenants.Where(p =>
            {
                var match = p.FolderName + ".";
                return host.Contains("." + match);
            }).FirstOrDefault();

            return tenant ?? Tenants[0];
        }
    }
}