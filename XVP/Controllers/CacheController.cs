using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XVP.Infrastructure.Shared.Cache;

namespace XVP.Presentation.MVC.Controllers
{
    public class CacheController : Controller
    {
        // GET: Cache
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Flush()
        {
           CacheService.Flush();
           return RedirectToAction("Index", "Home");
        }
    }
}