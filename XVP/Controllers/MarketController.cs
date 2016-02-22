namespace XVP.Presentation.MVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;

    using Microsoft.AspNet.Identity;

    using Newtonsoft.Json;

    using XVP.Domain.Models.Models;
    using XVP.Infrastructure.Query.Services;
    using XVP.Infrastructure.Shared.Enum;
    using XVP.Presentation.MVC.Services;

    public class MarketController : ADocumentController<Campaign>
    {
        public MarketController()
        {
            this.QueueService = new CloudQueueService(AzureQueues.Command);
            this.DocumentQueryService = new DocumentQueryService<Campaign>(this.CurrentTenant.Name);
        }

        // GET: Market
        public async Task<ActionResult> Index(string campaignId)
        {
           
            var httpCookie = this.Request.Cookies["campaign"];
            Campaign model = null;
            if (campaignId == null)
            {
                if (httpCookie != null && httpCookie["campaignId"] != null)
                {
                    campaignId = httpCookie["campaignId"];
                }
               
                model = campaignId != null ? this.GetById(campaignId) : new Campaign();
                
                if (model.MediaItems == null)
                {
                    model.MediaItems = new List<MediaItem>();
                }
            }
            else
            {
                httpCookie = new HttpCookie("campaign");
            }

            if (httpCookie != null)
            {
                httpCookie["campaignId"] = campaignId;
            }
            return View(model);
        }

       
        public async Task<ActionResult> AddToCampaign(string itemId)
        {
            if (itemId.IsEmpty())
            {
                return null;
            }

            var campaign = await this.GetCampaign();
            var item = this.GetById<MediaItem>(itemId);
            campaign.MediaItems.Add(item);
            await this.ReplaceAsync(campaign.Id, campaign);
            return Content(campaign.MediaItems.Count.ToString());
        }

        private async Task<Campaign> GetCampaign()
        {
            Campaign campaign = null;
            var httpCookie = this.Request.Cookies["campaign"];
            string campaignId = null;
            if (httpCookie != null && httpCookie["campaignId"] != null)
            {
                campaignId = httpCookie["campaignId"];
            }

            if (campaignId != null)
            {
                campaign = this.GetById(campaignId);
            }

            if (campaign != null)
            {
                return campaign;
            }

            string newCartId;
            var guid = Guid.NewGuid().ToString("N");
            var userId = this.User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                newCartId = userId + "-" + guid;
            }
            else
            {
                newCartId = "anon" + "-" + guid;
            }

            campaign = new Campaign() { Id = newCartId };
            await this.InsertAsync(campaign);
            httpCookie = new HttpCookie("campaign");
            httpCookie["campaignId"] = newCartId;
            this.Response.Cookies.Add(httpCookie);
            return campaign;
        }
    }
}