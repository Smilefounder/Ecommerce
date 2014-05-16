using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.API.Metadata;
using Kooboo.Commerce.CMSIntegration.DataSources.Models;
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
        private IResourceDescriptorProvider _descriptorProvider;

        public CommerceDataSourceController(IResourceDescriptorProvider descriptorProvider)
        {
            _descriptorProvider = descriptorProvider;
        }

        public ActionResult Queries()
        {
            var descriptors = QueryDescriptors.Descriptors.Select(x => new QueryDescriptorModel(x)).ToList();
            return Json(descriptors, JsonRequestBehavior.AllowGet);
        }
    }
}
