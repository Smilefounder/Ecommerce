using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.CMSIntegration.DataSources.Models;
using Kooboo.Commerce.CMSIntegration.DataSources.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Controllers
{
    [Authorize]
    public class CommerceDataSourceController : Controller
    {
        private IEnumerable<ICommerceSource> _sources;

        public CommerceDataSourceController(IEnumerable<ICommerceSource> sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            _sources = sources;
        }

        public ActionResult List()
        {
            var descriptors = _sources.OrderBy(x => x.Name)
                                      .Select(x => new CommerceSourceModel(x))
                                      .ToList();
            return Json(descriptors, JsonRequestBehavior.AllowGet);
        }
    }
}
