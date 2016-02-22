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
    using XVP.Domain.Models.ViewModels;
    using XVP.Infrastructure.Query.Services;
    using XVP.Infrastructure.Shared.Enum;
    using XVP.Presentation.MVC.Services;

    public class CampaignController : ADocumentController<Campaign>
    {
        public CampaignController()
        {
            this.DocumentQueryService = new DocumentQueryService<Campaign>(this.CurrentTenant.Name);
            this.QueueService = new CloudQueueService(AzureQueues.Command);
        }

        // GET: Tenant
        public ActionResult Index()
        {
            var model = DocumentQueryService.GetAll();
            return View(model);
        }

        // GET: Tenant/Details/5
        public ActionResult Details(string id)
        {
            var model = this.GetById(id);
            return View(model);
        }

       
        // GET: Tenant/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Tenant/Create
        [HttpPost]
        public async Task<ActionResult> Create(Campaign campaign)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    campaign.Id = Guid.NewGuid().ToString("N");
                    await this.InsertAsync(campaign);
                }
                else
                {
                    return View("Create", campaign);
                }

                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }

        
        public ActionResult Edit(string id)
        {
            var model = this.GetById(id);
            return View(model);
        }

        // POST: Tenant/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Campaign campaign)
        {
            try
            {
               if (this.ModelState.IsValid)
                {
                    await this.ReplaceAsync(campaign.Id, campaign);
                }
                else
                {
                    return View("Create", campaign);
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
                return this.RedirectToAction("Index");
            }
            catch
            {
                return this.View();
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateFromJson(Campaign campaign)
        {
            try
            {
                if (campaign.Id != null && campaign.Id != "0")
                {
                    await ReplaceAsync(campaign.Id, campaign);
                }
                else
                {
                    campaign.Id = Guid.NewGuid().ToString("N");
                    await this.InsertAsync(campaign);
                    return new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = "Success!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = ex.Message
                };
            }
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data ="Returned plain"
            };
           
        }

    }
}
