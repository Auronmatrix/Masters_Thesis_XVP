namespace XVP.Presentation.MVC.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using XVP.Domain.Commands;
    using XVP.Domain.Commands.Commands;
    using XVP.Domain.Models.Models;
    using XVP.Domain.Models.Orchastrator;
    using XVP.Infrastructure.Query.Services;
    using XVP.Infrastructure.Shared.Cache;
    using XVP.Infrastructure.Shared.Enum;
    using XVP.Presentation.MVC.Services;

    public class TenantController : ADocumentController<Tenant>
    {
        public TenantController()
        {
            this.QueueService = new CloudQueueService(AzureQueues.Command);
            this.DocumentQueryService = new DocumentQueryService<Tenant>(this.CurrentTenant.Name);
        }

        // GET: Tenant
        public ActionResult Index()
        {
            TenantService.Tenants = this.GetAll();
            var model = TenantService.Tenants;
            return View(model);
        }

        // GET: Tenant/Details/5
        public ActionResult Details(string id)
        {
            var model = this.GetById(id);
            return View(model);
        }

        public async Task<ActionResult> Initialize()
        {
            string json;
            using (var fs = new FileStream(this.Server.MapPath("~/Content/Startup/tenantSeed.json"), FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    json = sr.ReadToEnd();
                }
            }

            ICommand command = new InitializeTenantsCommand(json);

            foreach(var tenant in TenantService.Tenants)
            {
                if (tenant != null && tenant.Id != null)
                {
                    await CacheService.Cache.DeleteIfExists(tenant.Id);
                }
            }

            await this.QueueService.AddMessageAsync(command);
           
            TenantService.Tenants = this.GetAll();
            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView("_List", this.GetAll());
            }

            return this.RedirectToAction("Index");
        }

        // GET: Tenant/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Tenant/Create
        [HttpPost]
        public async Task<ActionResult> Create(Tenant tenant)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    tenant.Id = Guid.NewGuid().ToString("N");
                    await this.InsertAsync(tenant);
                    TenantService.Tenants = this.GetAll();
                }
                else
                {
                    return View("Create", tenant);
                }

                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }

        // GET: Tenant/Edit/5
        public ActionResult Edit(string id)
        {
            var model = this.GetById(id);
            return View(model);
        }

        // POST: Tenant/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Tenant tenant)
        {
            try
            {
               if (this.ModelState.IsValid)
                {
                    await this.ReplaceAsync(tenant.Id, tenant);
                    TenantService.Tenants = this.GetAll();
                }
                else
                {
                    return View("Create", tenant);
                }
                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }
       
       public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await this.DeleteAsync(id);
                TenantService.Tenants = this.GetAll();
                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }
    }
}
