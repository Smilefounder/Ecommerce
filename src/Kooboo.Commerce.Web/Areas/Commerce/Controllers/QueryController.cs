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
            var queryInfo = manager.GetQueryInfo(queryName);
            var queryConfig = queryInfo.GetQueryConfig();
            var config = queryConfig ?? TypeActivator.CreateInstance(queryInfo.Query.ConfigType);

            ViewBag.QueryInfo = queryInfo;
            ViewBag.DisplayName = queryInfo.GetDisplayName();

            return PartialView(config);
        }

        [HttpPost, HandleAjaxError]
        public void Config(string queryName, [ModelBinder(typeof(ObjectModelBinder))]object config)
        {
            var manager = QueryManager.Instance;
            var queryInfo = manager.GetQueryInfo(queryName);

            queryInfo.SetDisplayName(Request.Form["DisplayName"]);
            queryInfo.WriteQueryConfig(config);
        }
    }
}
