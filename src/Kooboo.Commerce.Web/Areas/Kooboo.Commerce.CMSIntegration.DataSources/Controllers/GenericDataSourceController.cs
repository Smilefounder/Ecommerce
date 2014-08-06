using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.CMSIntegration.DataSources.Generic;
using Kooboo.Commerce.CMSIntegration.DataSources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Controllers
{
    [Authorize]
    public class GenericDataSourceController : Controller
    {
        private List<GenericCommerceDataSource> _dataSources;

        public GenericDataSourceController(IEnumerable<ICommerceDataSource> dataSources)
        {
            if (dataSources == null)
                throw new ArgumentNullException("dataSources");

            _dataSources = dataSources.OfType<GenericCommerceDataSource>().ToList();
        }

        public ActionResult List()
        {
            var descriptors = _dataSources.OrderBy(x => x.Name)
                                      .Select(x => new GenericDataSourceModel(x))
                                      .ToList();
            return Json(descriptors, JsonRequestBehavior.AllowGet);
        }
    }
}
