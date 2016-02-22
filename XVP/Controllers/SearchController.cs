namespace XVP.Presentation.MVC.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using AzureSearchClient;

    using XVP.Domain.Models.Models;
    using XVP.Infrastructure.Command.Initializers;
    using XVP.Infrastructure.Query.Services;
    using XVP.Infrastructure.Shared.CustomAttributes;

    using SearchableMediaItem = XVP.Domain.Models.ViewModels.SearchableMediaItem;

    public class SearchController : BaseMultiTenantController
    {
        private static int items = 0;

        private SearchQueryService<SearchableMediaItem> service;

        public SearchController()
        {
            service = new SearchQueryService<SearchableMediaItem>();
           
        }

        public async Task<ActionResult> InitMedia()
        {
            string json;
            using (var fs = new FileStream(this.Server.MapPath("~/Content/Startup/searchSeed.json"), FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    json = sr.ReadToEnd();
                }
            }

            var attribute =
                typeof(MediaItem).GetCustomAttributes(typeof(DocumentAttribute), true).FirstOrDefault() as
                DocumentAttribute;
            if (attribute == null)
            {
                throw new ArgumentException("Model does not implement custom attribute 'Document'");
            }

            var collectionId = attribute.CollectionId;
            var databaseId = attribute.DatabaseId;

            await SearchDbInitializer<SearchableMediaItem>.Initialize(json, "media");
            return this.View("InitStatus", true);
        }

        public ActionResult AddToCart(string itemId)
        {
            items++;
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = "{ count : " + items + "}"
                };
        }
        
        public async Task<ActionResult> GetMediaWithText(double distance = 40)
        {
            await service.LoadIndexesAsync();
            var query = new QueryParameters
                            {
                                Filter = String.Format("(Tenants/any(t: t eq '{0}')) and (geo.distance(Location, geography'POINT(16.6068371 49.1950602)') le {1})", this.CurrentTenant.TenantId, distance)
                            };
            var result = await service.QueryAsync("media", query);
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public async Task<ActionResult> GetMediaWithFilter(string search, int distance)
        {
            await service.LoadIndexesAsync();
            var query = new QueryParameters
                            {
                                QueryText = search,
                                SearchFields = new string[] { "Type" },
                                Filter = String.Format("(Tenants/any(t: t eq '{0}')) and (geo.distance(Location, geography'POINT(16.6068371 49.1950602)') le {1})", this.CurrentTenant.TenantId, distance)
                            };
            var result = await service.QueryAsync("media", query);
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }

        public async Task<JsonResult> GetMediaItem(string itemId)
        {
            await service.LoadIndexesAsync();
            var result = await service.Lookup("media", itemId);
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }


    }
}