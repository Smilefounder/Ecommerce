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
            var descriptorsByCategories = _resourceDescriptorProvider.GetAllDescriptors()
                                                                     .ToList()
                                                                     .GroupBy(x => x.ResourceName.Category, x => new ResourceModel(x));

            return View(descriptorsByCategories);
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
                var destinationResource = _resourceDescriptorProvider.GetDescriptor(link.DestinationResourceName);
                var linkModel = new ResourceLinkModel(link, destinationResource);
                result.Add(linkModel);
            }

            return JsonNet(result).UsingClientConvention();
        }

        public ActionResult Resource(string resourceName)
        {
            var descriptor = _resourceDescriptorProvider.GetDescriptor(resourceName);
            var model = new ResourceModel(descriptor);
            return JsonNet(model).UsingClientConvention();
        }

        public ActionResult ResourceLink(string linkId)
        {
            var link = _resourceLinkPersistence.GetById(linkId);
            var destinationResource = _resourceDescriptorProvider.GetDescriptor(link.DestinationResourceName);
            var model = new ResourceLinkModel(link, destinationResource);
            return JsonNet(model).UsingClientConvention();
        }

        public ActionResult ResourceCategories()
        {
            var categories = _resourceDescriptorProvider.GetAllDescriptors()
                                                        .ToList()
                                                        .GroupBy(x => x.ResourceName.Category)
                                                        .Select(x => new
                                                        {
                                                            Name = x.Key,
                                                            Resources = x.Select(it => new ResourceModel(it))
                                                        })
                                                        .ToList();

            return JsonNet(categories).UsingClientConvention();
        }

        public ActionResult LoadLinkParametersWithDefault(string sourceResourceName, string destinationResourceName)
        {
            var sourceResource = _resourceDescriptorProvider.GetDescriptor(sourceResourceName);
            var destinationResource = _resourceDescriptorProvider.GetDescriptor(destinationResourceName);

            var requiredParams = new List<ResourceLinkParameterModel>();
            var optionalParams = new List<ResourceLinkParameterModel>();

            foreach (var param in destinationResource.InputPramameters)
            {
                var paramModel = new ResourceLinkParameterModel
                {
                    Name = param.Name,
                    ParameterType = param.ParameterType.Name,
                    Required = param.Required
                };

                if (param.Required)
                {
                    // Populate default value for required parameters
                    var fromParam = sourceResource.OutputParameters
                                                  .FirstOrDefault(p =>
                                                      p.ParameterType == param.ParameterType
                                                      && ResourceLinkParameterModel.GetParameterDisplayName(p.Name).Equals(paramModel.DisplayName, StringComparison.OrdinalIgnoreCase));

                    if (fromParam != null)
                    {
                        paramModel.Value = fromParam.Name;
                    }

                    requiredParams.Add(paramModel);
                }
                else
                {
                    optionalParams.Add(paramModel);
                }
            }

            return JsonNet(new
            {
                RequiredParameters = requiredParams,
                OptionalParameters = optionalParams
            })
            .UsingClientConvention();
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
