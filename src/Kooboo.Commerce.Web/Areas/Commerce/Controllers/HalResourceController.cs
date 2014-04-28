using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.API.HAL.Persistence;
using Kooboo.Commerce.Web.Areas.Commerce.Models.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class HalResourceController : CommerceControllerBase
    {
        private IResourceDescriptorProvider _resourceDescriptorProvider;
        private IResourceLinkPersistence _resourceLinkPersistence;
        private IEnumerable<IContextEnviromentProvider> _environmentProviders;

        public HalResourceController(IEnumerable<IContextEnviromentProvider> environmentProviders, IResourceDescriptorProvider resourceDescriptorProvider, IResourceLinkPersistence resourceLinkPersistence)
        {
            _environmentProviders = environmentProviders;
            _resourceDescriptorProvider = resourceDescriptorProvider;
            _resourceLinkPersistence = resourceLinkPersistence;
        }

        public ActionResult List()
        {
            var descriptors = _resourceDescriptorProvider.GetAllDescriptors()
                                                         .Select(x => new ResourceModel(x))
                                                         .ToList();
            return View(descriptors);
        }

        public ActionResult Detail(string resourceName)
        {
            var resource = _resourceDescriptorProvider.GetDescriptor(resourceName);
            var linkableResources = _resourceDescriptorProvider.GetAllDescriptors();

            if (resource.IsListResource && !String.IsNullOrEmpty(resource.ItemResourceName))
            {
                var itemResource = _resourceDescriptorProvider.GetDescriptor(resource.ItemResourceName);
                ViewBag.ItemResource = itemResource;
            }

            return View(resource);
        }

        public ActionResult Links(string resourceName)
        {
            var result = new List<ResourceLinkModel>();
            var links = _resourceLinkPersistence.GetLinks(resourceName);
            foreach (var link in links)
            {
                var linkedResource = _resourceDescriptorProvider.GetDescriptor(link.DestinationResourceName);
                var linkModel = new ResourceLinkModel(link);
                result.Add(linkModel);
            }

            return JsonNet(result).UsingClientConvention();
        }

        public ActionResult Resource(string resourceName)
        {
            var descriptor = _resourceDescriptorProvider.GetDescriptor(resourceName);
            var model = new ResourceModel(descriptor);

            // TODO: Fake
            model.OutputParameters.Add(new HalParameterModel
            {
                Name = "page"
            });
            model.OutputParameters.Add(new HalParameterModel
            {
                Name = "brandId"
            });

            return JsonNet(model).UsingClientConvention();
        }

        public ActionResult ResourceLink(string linkId)
        {
            var link = _resourceLinkPersistence.GetById(linkId);
            var model = new ResourceLinkModel(link);
            return JsonNet(model).UsingClientConvention();
        }

        public ActionResult LinkableResources()
        {
            var resources = _resourceDescriptorProvider.GetAllDescriptors()
                                                       .Select(x => new ResourceModel(x));

            return JsonNet(resources).UsingClientConvention();
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult SaveLink(string sourceResourceName, ResourceLinkModel linkModel)
        {
            ResourceLink link = null;

            if (!String.IsNullOrEmpty(linkModel.Id))
            {
                link = _resourceLinkPersistence.GetById(linkModel.Id);
                linkModel.UpdateTo(link);
                _resourceLinkPersistence.Save(link);
            }
            else
            {
                link = new ResourceLink
                {
                    SourceResourceName = sourceResourceName
                };
                linkModel.UpdateTo(link);
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
