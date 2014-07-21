using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class TabQueryController : CommerceController
    {
        public ActionResult Config(string pageName, string queryId, string queryName)
        {
            var manager = new SavedTabQueryManager();
            ITabQuery query = null;
            SavedTabQuery savedQuery = null;

            if (!String.IsNullOrEmpty(queryId))
            {
                savedQuery = manager.Find(pageName, new Guid(queryId));
                query = TabQueries.GetQuery(savedQuery.QueryName);
                if (savedQuery.Config == null && query.ConfigType != null)
                {
                    savedQuery.Config = Activator.CreateInstance(query.ConfigType);
                }
            }
            else
            {
                query = TabQueries.GetQuery(queryName);
                savedQuery = new SavedTabQuery(queryName);
                if (query.ConfigType != null)
                {
                    savedQuery.Config = Activator.CreateInstance(query.ConfigType);
                }
            }

            ViewBag.Query = query;

            return PartialView(savedQuery);
        }

        [HttpPost, HandleAjaxError]
        public ActionResult Config(string pageName, string queryName, string queryId, [ModelBinder(typeof(BindingTypeAwareModelBinder))]object config)
        {
            var manager = new SavedTabQueryManager();
            ITabQuery query = null;
            SavedTabQuery savedQuery = null;

            if (!String.IsNullOrEmpty(queryId))
            {
                savedQuery = manager.Find(pageName, new Guid(queryId));
                query = TabQueries.GetQuery(savedQuery.QueryName);
            }
            else
            {
                savedQuery = new SavedTabQuery(queryName);
                query = TabQueries.GetQuery(queryName);
            }

            savedQuery.DisplayName = Request.Form["DisplayName"];

            if (query.ConfigType != null)
            {
                savedQuery.Config = config;
            }

            if (!String.IsNullOrEmpty(queryId))
            {
                manager.Update(pageName, savedQuery);
            }
            else
            {
                manager.Add(pageName, savedQuery);
            }

            return Json(savedQuery);
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
