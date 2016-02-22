namespace XVP.Presentation.MVC.Controllers
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    using XVP.Domain.Models.Models;
    using XVP.Domain.Models.Orchastrator;

    public abstract class BaseMultiTenantController : Controller
    {
        protected BaseMultiTenantController()
        {
            this.CurrentTenant = TenantService.GetCurrentTenant(null);
        }

        public Tenant CurrentTenant { get; set; }
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Debug.Assert(filterContext.HttpContext.Request.Url != null, "filterContext.HttpContext.Request.Url != null");
            this.CurrentTenant = TenantService.GetCurrentTenant(filterContext.HttpContext.Request.Url.Host.ToLower());
            this.ViewBag.CurrentTenant = this.CurrentTenant;
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1122:UseStringEmptyForEmptyStrings", Justification = "Reviewed. Suppression is OK here.")]
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null)
            {
                viewResult.MasterName = viewResult.MasterName != "IGNORE" ? "_Layout" : "";
            }

            Debug.Assert(filterContext.HttpContext.Request.Url != null, "filterContext.HttpContext.Request.Url != null");
            this.CurrentTenant = TenantService.GetCurrentTenant(filterContext.HttpContext.Request.Url.Host.ToLower());
        }
    }
}