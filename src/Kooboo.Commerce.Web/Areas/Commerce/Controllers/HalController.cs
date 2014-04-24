using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.API.HAL.Persistence;
using Kooboo.Commerce.Web.Areas.Commerce.Models.HAL;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Globalization;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class HalController : CommerceControllerBase
    {
        private IResourceDescriptorProvider _resourceDescriptorProvider;
        private IResourceLinkPersistence _resourceLinkPersistence;
        private IEnumerable<IContextEnviromentProvider> _environmentProviders;

        public HalController(IEnumerable<IContextEnviromentProvider> environmentProviders, IResourceDescriptorProvider resourceDescriptorProvider, IResourceLinkPersistence resourceLinkPersistence)
        {
            _environmentProviders = environmentProviders;
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
            var linkableResources = _resourceDescriptorProvider.GetAllDescriptors();

            var model = new ResourceDetailModel
            {
                ResourceName = resource.ResourceName,
                ResourceUri = resource.ResourceUri,
                IsListResource = resource.IsListResource,
                LinkableResources = linkableResources.Select(x => new ResourceModel(x)).ToList(),
                ImplicitLinkProvider = resource.ImplicitLinkProvider
            };

            model.Environments.Add(new SelectListItem
            {
                Text = "Any".Localize(),
                Value = ""
            });

            foreach (var provider in _environmentProviders)
            {
                model.Environments.Add(new SelectListItem
                {
                    Text = provider.Name,
                    Value = provider.Name
                });
            }

            if (resource.IsListResource && !String.IsNullOrEmpty(resource.ItemResourceName))
            {
                var itemResource = _resourceDescriptorProvider.GetDescriptor(resource.ItemResourceName);
                model.ItemResource = new ResourceModel(itemResource);
            }

            var links = _resourceLinkPersistence.GetLinks(resource.ResourceName, null);
            foreach (var link in links)
            {
                var linkedResource = _resourceDescriptorProvider.GetDescriptor(link.DestinationResourceName);
                var linkModel = new ResourceLinkModel
                {
                    Id = link.Id,
                    Relation = link.Relation,
                    DestinationResource = new ResourceModel(linkedResource)
                };

                model.Links.Add(linkModel);
            }

            return View(model);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult SaveLink(string sourceResourceName, ResourceLinkModel linkModel)
        {
            ResourceLink link = null;

            if (!String.IsNullOrEmpty(linkModel.Id))
            {
                link = _resourceLinkPersistence.GetById(linkModel.Id);
                link.Relation = linkModel.Relation;
                link.DestinationResourceName = linkModel.DestinationResource.ResourceName;
                link.EnvironmentName = linkModel.EnvironmentName;
                _resourceLinkPersistence.Save(link);
            }
            else
            {
                link = new ResourceLink
                {
                    Relation = linkModel.Relation,
                    SourceResourceName = sourceResourceName,
                    DestinationResourceName = linkModel.DestinationResource.ResourceName,
                    EnvironmentName = linkModel.EnvironmentName
                };
                _resourceLinkPersistence.Save(link);
            }

            return AjaxForm().Success().WithModel(new
            {
                Id = link.Id
            });
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult DeleteLink(string linkId)
        {
            _resourceLinkPersistence.Delete(linkId);
            return AjaxForm().Success();
        }
    }
}
