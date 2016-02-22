namespace XVP.Presentation.MVC
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using XVP.Domain.Models.Models;
    using XVP.Domain.Models.Orchastrator;
    using XVP.Infrastructure.Query.Services;
    using XVP.Presentation.MVC.ViewEngines;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           // WebApiConfig.Register(GlobalConfiguration.Configuration);
           
            // Configure the MVC view Engine as the core one
            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(new MulitTenantRazorViewEngine());
            var documentService = new DocumentQueryService<Tenant>();
            TenantService.Tenants = documentService.GetAll();

        }
    }
}
