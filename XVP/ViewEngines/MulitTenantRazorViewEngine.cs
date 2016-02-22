// This custom view Engine is based on  Ben Morris' article that can be found at http://lonetechie.com/2012/09/25/multi-tenant-architecture-with-asp-net-mvc-4/

namespace XVP.Presentation.MVC.ViewEngines
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;

    using XVP.Infrastructure.Shared.Logging;
    using XVP.Presentation.MVC.Controllers;

    public class MulitTenantRazorViewEngine : RazorViewEngine
    {
        public MulitTenantRazorViewEngine()
        {
            this.AreaViewLocationFormats = new[] 
            {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };

            this.AreaMasterLocationFormats = new[] 
            {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };

            this.AreaPartialViewLocationFormats = new[] 
            {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };

            this.ViewLocationFormats = new[] 
            {
            "~/Views/%1/{1}/{0}.cshtml",
            "~/Views/%1/{1}/{0}.vbhtml",
            "~/Views/%1/Shared/{0}.cshtml",
            "~/Views/%1/Shared/{0}.vbhtml",
            "~/Views/Global/{1}/{0}.cshtml",
            "~/Views/Global/{1}/{0}.vbhtml",
            "~/Views/Global/Shared/{0}.cshtml",
            "~/Views/Global/Shared/{0}.vbhtml"
            };

            this.MasterLocationFormats = new[] 
            {
            "~/Views/%1/{1}/{0}.cshtml",
            "~/Views/%1/{1}/{0}.vbhtml",
            "~/Views/%1/Shared/{0}.cshtml",
            "~/Views/%1/Shared/{0}.vbhtml",
            "~/Views/Global/{1}/{0}.cshtml",
            "~/Views/Global/{1}/{0}.vbhtml",
            "~/Views/Global/Shared/{0}.cshtml",
            "~/Views/Global/Shared/{0}.vbhtml"
            };

            this.PartialViewLocationFormats = new[] 
            {
            "~/Views/%1/{1}/{0}.cshtml",
            "~/Views/%1/{1}/{0}.vbhtml",
            "~/Views/%1/Shared/{0}.cshtml",
            "~/Views/%1/Shared/{0}.vbhtml",
            "~/Views/Global/{1}/{0}.cshtml",
            "~/Views/Global/{1}/{0}.vbhtml",
            "~/Views/Global/Shared/{0}.cshtml",
            "~/Views/Global/Shared/{0}.vbhtml"
            };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            var passedController = controllerContext.Controller as BaseMultiTenantController;
            Debug.Assert(passedController != null, "PassedController != null");
            return base.CreatePartialView(controllerContext, partialPath.Replace("%1", passedController.CurrentTenant.FolderName));
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var passedController = controllerContext.Controller as BaseMultiTenantController;
            Debug.Assert(passedController != null, "PassedController != null");
            return base.CreateView(controllerContext, viewPath.Replace("%1", passedController.CurrentTenant.FolderName), masterPath.Replace("%1", passedController.CurrentTenant.FolderName));
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            try
            {
                var passedController = controllerContext.Controller as BaseMultiTenantController;
                if (passedController != null)
                {
                    return base.FileExists(controllerContext, virtualPath.Replace("%1", passedController.CurrentTenant.FolderName));
                }

                var newEx = new Exception("PassedController is null, Controller must inherit MultiTenantController");
                ElmahLogger.LogError(newEx);
                return base.FileExists(controllerContext, virtualPath);
            }
            catch (HttpException exception)
            {
                ElmahLogger.LogError(exception);
                if (exception.GetHttpCode() != 0x194)
                {
                    throw;
                }

                return false;
            }
            catch (Exception exception)
            {
                var newEx = new Exception((controllerContext == null).ToString(), exception);
                ElmahLogger.LogError(newEx);
                return false;
            }
        }
    }
}