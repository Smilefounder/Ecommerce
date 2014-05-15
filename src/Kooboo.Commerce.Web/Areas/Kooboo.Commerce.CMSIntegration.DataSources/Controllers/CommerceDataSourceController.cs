using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.CMSIntegration.DataSources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Controllers
{
    public class CommerceDataSourceController : Controller
    {
        private IResourceDescriptorProvider _descriptorProvider;

        public CommerceDataSourceController(IResourceDescriptorProvider descriptorProvider)
        {
            _descriptorProvider = descriptorProvider;
        }

        public ActionResult ResourceCategories()
        {
            var descriptors = _descriptorProvider.GetAllDescriptors()
                                                 .GroupBy(x => x.ResourceName.Category)
                                                 .Select(x => new
                                                 {
                                                     Name = x.Key,
                                                     Resources = x.Select(r => new ResourceModel(r)).ToList()
                                                 })
                                                 .ToList();

            return Json(descriptors, JsonRequestBehavior.AllowGet);
        }
    }
}
