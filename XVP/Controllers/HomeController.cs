namespace XVP.Presentation.MVC.Controllers
{
    using System.Web.Mvc;

    public class HomeController : BaseMultiTenantController
    {
        public ActionResult Index()
        {
           return this.View();
        }

        public ActionResult About()
        {
            this.ViewBag.Message = "Your application description page.";

            return this.View();
        }

        public ActionResult ComingSoon()
        {
            return this.View("ComingSoon");
        }

        public ActionResult Tech()
        {
            return View();
        }
    }
}