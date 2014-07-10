using Kooboo.Commerce.Web.Framework.Queries;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class QueryController : CommerceControllerBase
    {
        public ActionResult Config(string queryName)
        {
            var manager = QueryManager.Instance;
            var query = manager.GetQuery(queryName);
            var config = manager.GetQueryConfig(queryName) ?? TypeActivator.CreateInstance(query.ConfigType);

            ViewBag.Query = query;

            return PartialView(config);
        }

        [HttpPost, HandleAjaxError]
        public void Config(string queryName, [ModelBinder(typeof(ObjectModelBinder))]object config)
        {
            var manager = QueryManager.Instance;
            manager.SaveQueryConfig(queryName, config);
        }
    }
}
