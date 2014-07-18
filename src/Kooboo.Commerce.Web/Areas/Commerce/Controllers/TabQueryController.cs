using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.ModelBinding;
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
            var savedQuery = manager.GetSavedQuery(pageName, new Guid(queryId));

            ViewBag.Query = TabQueries.GetQuery(savedQuery.QueryName);

            return PartialView(savedQuery);
        }

        [HttpPost, HandleAjaxError]
        public void Config(string pageName, [ModelBinder(typeof(BindingTypeAwareModelBinder))]SavedTabQuery model)
        {
            var manager = new SavedTabQueryManager();
            manager.UpdateSavedQuery(pageName, model);
        }
    }
}
