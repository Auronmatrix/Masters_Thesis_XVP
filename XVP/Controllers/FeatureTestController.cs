namespace XVP.Presentation.MVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using XVP.Domain.Commands;
    using XVP.Domain.Commands.Commands;
    using XVP.Domain.Models.Models;
    using XVP.Domain.Models.ViewModels;
    using XVP.Infrastructure.Command.Initializers;
    using XVP.Infrastructure.Command.Services;
    using XVP.Infrastructure.Query.Services;
    using XVP.Infrastructure.Shared.CustomAttributes;
    using XVP.Infrastructure.Shared.Enum;
    using XVP.Presentation.MVC.Services;

    public class FeatureTestController : ADocumentController<SeedModel>
    {
        public FeatureTestController()
        {
            this.QueueService = new CloudQueueService(AzureQueues.Command);
            this.DocumentQueryService = new DocumentQueryService<SeedModel>();
        }

        public async Task<ActionResult> InitDocumentDb()
        {
            string json;
            using (var fs = new FileStream(this.Server.MapPath("~/Content/Startup/seed.json"), FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    json = sr.ReadToEnd();
                }
            }

            var status = await DocumentDbInitializer<SeedModel>.Initialize(json);
            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView("_List", this.GetAll());
            }

            return this.View("InitStatus", status);
        }

        

        public async Task Replace(string id, SeedModel item)
        {
            await this.ReplaceAsync(id, item);
        }

        public ActionResult Index()
        {
            var model = this.GetAll();
            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView("_list", model);
            }

            return this.View("Index", model);
        }

        public ActionResult Details(string id)
        {
            var model = this.GetById(id);
            return this.View("Details", model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            await this.DeleteAsync(id);
            return this.RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(SeedModel model)
        {
            await this.InsertAsync(model);
            return this.RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var model = this.GetById(id);
            return this.View("Edit", model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(SeedModel model)
        {
            await this.ReplaceAsync(model.Id, model);
            return this.RedirectToAction("Index");
        }

        public async Task<ActionResult> InitSearchDb()
        {
            string json;
            using (var fs = new FileStream(this.Server.MapPath("~/Content/Startup/searchSeed.json"), FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    json = sr.ReadToEnd();
                }
            }

            await SearchDbInitializer<SearchableMediaItem>.Initialize(json, "media");
            return this.View("InitSearchDb");
        }

        public async Task<ActionResult> AddMessage()
        {
            var rnd = new Random();
            var cartName = rnd.Next(0, 99999);

                var campaignCart = new CampaignCart() { Id = Guid.NewGuid().ToString("N"), Items = new List<MediaItem>() };
               
                var queueService = new CloudQueueService(AzureQueues.Command);
            for (var k = 0; k < 5; k++)
            {
                 var type = rnd.Next(0, 99999);
                var mediaItem = new MediaItem()
                                    {
                                        Description = "Some random description number " + type,
                                        Name = "Random Name " + type,
                                        Id = Guid.NewGuid().ToString("N"),
                                        Price = type * type
                                    };
                campaignCart.Items.Add(mediaItem);
            }
                ICommand command = new CreateCampaignCartCommand(this.CurrentTenant.Name, campaignCart);
                await queueService.AddMessageAsync(command);
            return this.RedirectToAction("Index");

        }
    }
}