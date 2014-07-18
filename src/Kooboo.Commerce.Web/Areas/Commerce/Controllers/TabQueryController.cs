using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using Kooboo.Commerce.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class TabQueryController : CommerceControllerBase
    {
        public ActionResult Config(string pageName, string queryId)
        {
            var manager = new SavedTabQueryManager();
            var savedQuery = manager.Find(pageName, new Guid(queryId));
            var query = TabQueries.GetQuery(savedQuery.QueryName);
            if (savedQuery.Config == null)
            {
                savedQuery.Config = Activator.CreateInstance(query.ConfigType);
            }

            ViewBag.Query = query;

            return PartialView(savedQuery);
        }

        [HttpPost, HandleAjaxError]
        public ActionResult Config(string pageName, string queryId, [ModelBinder(typeof(BindingTypeAwareModelBinder))]object config)
        {
            var manager = new SavedTabQueryManager();
            var savedQuery = manager.Find(pageName, new Guid(queryId));
            savedQuery.DisplayName = Request.Form["DisplayName"];
            savedQuery.Config = config;
            manager.Update(pageName, savedQuery);
            return Json(savedQuery);
        }

        [HttpPost, HandleAjaxError]
        public ActionResult Add(string pageName, string queryName)
        {
            var manager = new SavedTabQueryManager();
            var query = TabQueries.GetQuery(queryName);
            var savedQuery = SavedTabQuery.CreateFrom(query);
            manager.Add(pageName, savedQuery);

            return Json(new
            {
                QueryId = savedQuery.Id.ToString(),
                DisplayName = savedQuery.DisplayName
            });
        }

        [HttpPost, HandleAjaxError]
        public void Delete(string pageName, Guid queryId)
        {
            var manager = new SavedTabQueryManager();
            manager.Delete(pageName, queryId);
        }

        [HttpPost]
        public void SaveOrders(string pageName, SavedTabQuery[] models)
        {
            var manager = new SavedTabQueryManager();
            foreach (var model in models)
            {
                var query = manager.Find(pageName, model.Id);
                query.Order = model.Order;
                manager.Update(pageName, query);
            }
        }
    }
}
