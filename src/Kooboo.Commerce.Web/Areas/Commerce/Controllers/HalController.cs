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
using Kooboo.Commerce.API.HAL.Services;
using Kooboo.Extensions;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class HalController : CommerceControllerBase
    {
        private IResourceDescriptorProvider _resourceDescriptorProvider;
        private IResourceLinkPersistence _resourceLinkPersistence;
        private IEnumerable<IContextEnviromentProvider> _environmentProviders;
        private IHalRuleService _halRuleService;

        public HalController(IEnumerable<IContextEnviromentProvider> environmentProviders, IResourceDescriptorProvider resourceDescriptorProvider, IResourceLinkPersistence resourceLinkPersistence, IHalRuleService halRuleService)
        {
            _environmentProviders = environmentProviders;
            _resourceDescriptorProvider = resourceDescriptorProvider;
            _resourceLinkPersistence = resourceLinkPersistence;
            _halRuleService = halRuleService;
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

        public ActionResult ResourceRules(int? page, int? pageSize)
        {
            var rules = _halRuleService.Query()
                                .OrderByDescending(x => x.Id)
                                .ToPagedList(page, pageSize)
                                .Transform(x => new HalRuleRowModel(x));

            return View(rules);
        }

        public ActionResult CreateRule()
        {
            var model = new HalRuleEditorModel();
            ViewBag.CurrentEventType = typeof(HalContext).AssemblyQualifiedNameWithoutVersion();
            ViewBag.Resources = _resourceDescriptorProvider.GetAllDescriptors();
            return View(model);
        }

        public ActionResult EditRule(int id)
        {
            var rule = _halRuleService.GetById(id);
            var model = new HalRuleEditorModel(rule);
            ViewBag.CurrentEventType = typeof(HalContext).AssemblyQualifiedNameWithoutVersion();
            ViewBag.Resources = _resourceDescriptorProvider.GetAllDescriptors();
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult SaveRule(HalRuleEditorModel model, string @return)
        {
            var resources = Request.Form["Resources"].Split(',');
            foreach(var res in resources)
            {
                model.Resources.Add(new HalRuleResourceModel()
                {
                    RuleId = model.Id,
                    ResourceName = res
                });
            }

            HalRule rule = new HalRule();
            model.UpdateTo(rule);
            _halRuleService.Save(rule);

            return AjaxForm().RedirectTo(@return);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult DeleteRule(HalRuleRowModel[] model)
        {
            foreach (var m in model)
            {
                var brand = _halRuleService.GetById(m.Id);
                _halRuleService.Delete(brand);
            }

            return AjaxForm().ReloadPage();
        }

        public ActionResult HighlightConditionsExpression(string expression)
        {
            return JsonNet(new
            {
                ConditionsExpression = expression,
                HighlightedConditionsExpression = new ConditionsExpressionPrettifier().Prettify(expression, typeof(HalContext))
            }).UsingClientConvention();
        }
    }
}
