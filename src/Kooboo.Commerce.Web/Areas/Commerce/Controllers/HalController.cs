using Kooboo.Commerce.HAL;
using Kooboo.Commerce.HAL.Persistence;
using Kooboo.Commerce.Web.Areas.Commerce.Models.HAL;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class HalController : CommerceControllerBase
    {
        private IResourceDescriptorProvider _resourceDescriptorProvider;
        private IResourceLinkPersistence _resourceLinkPersistence;

        public HalController(IResourceDescriptorProvider resourceDescriptorProvider, IResourceLinkPersistence resourceLinkPersistence)
        {
            _resourceDescriptorProvider = resourceDescriptorProvider;
            _resourceLinkPersistence = resourceLinkPersistence;
        }

        public ActionResult Resources()
        {
            var descriptors = _resourceDescriptorProvider.GetAllDescriptors()
                                                         .Select(x => new ResourceModel(x))
                                                         .ToList();
            return View(descriptors);
        }

        public ActionResult Resource(string resourceName)
        {
            var resource = _resourceDescriptorProvider.GetDescriptor(resourceName);
            var linkableResources = _resourceDescriptorProvider.GetLinkableResources(resourceName);

            var model = new ResourceDetailModel
            {
                ResourceName = resource.ResourceName,
                ResourceUri = resource.ResourceUri,
                LinkableResources = linkableResources.Select(x => new ResourceModel(x)).ToList()
            };

            var links = _resourceLinkPersistence.GetLinks(resource.ResourceName);
            foreach (var link in links)
            {
                var linkedResource = _resourceDescriptorProvider.GetDescriptor(link.DestinationResourceName);
                var linkModel = new ResourceLinkModel
                {
                    Id = link.Id,
                    Rel = link.Rel,
                    DestinationResource = new ResourceModel(linkedResource)
                };

                model.Links.Add(linkModel);
            }

            return View(model);
        }
    }
}
