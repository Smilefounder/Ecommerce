using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.HAL.Persistence;
using Kooboo.Commerce.API.HAL.Services;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    [Dependency(typeof(IHalWrapper))]
    public class HalWrapper : IHalWrapper
    {
        private IUriResolver _uriResolver;
        private IResourceLinkPersistence _resourceLinkPersistence;
        private IResourceDescriptorProvider _resourceDescriptorProvider;
        private IEnumerable<IContextEnviromentProvider> _environmentProviders;
        private RuleEngine _ruleEngine;
        private IEnumerable<HalRule> _halRules;
        private IHalRuleService _halRuleService;

        public HalWrapper(
            IResourceDescriptorProvider resourceDescriptorProvider,
            IResourceLinkPersistence resourceLinkPersistence,
            IUriResolver uriResolver,
            IEnumerable<IContextEnviromentProvider> environmentProviders,
            IHalRuleService halRuleService,
            RuleEngine ruleEngine)
        {
            _resourceDescriptorProvider = resourceDescriptorProvider;
            _resourceLinkPersistence = resourceLinkPersistence;
            _uriResolver = uriResolver;
            _environmentProviders = environmentProviders;
            _halRuleService = halRuleService;
            _ruleEngine = ruleEngine;
        }

        public void AddLinks(string resourceName, IItemResource resource, HalContext context, IDictionary<string, object> parameterValues)
        {
            if (String.IsNullOrEmpty(resourceName))
                throw new ArgumentException("Resource name is required.", "resourceName");

            if (resource == null)
                throw new ArgumentNullException("resource");

            var descriptor = _resourceDescriptorProvider.GetDescriptor(resourceName);
            AssertDescriptorNotNull(descriptor, resourceName);
            var halRules = GetHalRulesByHalContext(context);
            FillLinksNoRecursive(descriptor, resource, parameterValues, halRules);
        }

        public void AddLinks<T>(string resourceName, IListResource<T> resource, HalContext context, IDictionary<string, object> parameterValues, Func<T, IDictionary<string, object>> itemParameterValuesResolver)
            where T : IItemResource
        {
            if (String.IsNullOrEmpty(resourceName))
                throw new ArgumentException("Resource name is required.", "resourceName");

            if (resource == null)
                throw new ArgumentNullException("resource");

            var descriptor = _resourceDescriptorProvider.GetDescriptor(resourceName);
            AssertDescriptorNotNull(descriptor, resourceName);

            var halRules = GetHalRulesByHalContext(context);
            FillLinksNoRecursive(descriptor, resource, parameterValues, halRules);


            var itemDescriptor = _resourceDescriptorProvider.GetDescriptor(descriptor.ItemResourceName);
            AssertDescriptorNotNull(itemDescriptor, descriptor.ItemResourceName);

            if (IsResourceInRules(descriptor.ItemResourceName, halRules))
            {
                foreach (var item in resource)
                {
                    var itemParamValues = itemParameterValuesResolver == null ? null : itemParameterValuesResolver(item);
                    FillLinksNoRecursive(itemDescriptor, item, itemParamValues, halRules);
                }
            }
        }

        private IEnumerable<HalRule> GetHalRulesByHalContext(HalContext context)
        {
            if (_halRules == null)
                _halRules = _halRuleService.Query().ToArray();
            return _halRules.Where(o => _ruleEngine.CheckCondition(o.ConditionsExpression, context));
        }

        private bool IsResourceInRules(string resourceName, IEnumerable<HalRule> halRules)
        {
            if (halRules == null || halRules.Count() <= 0)
                return true;
            foreach(var rule in halRules)
            {
                if (rule.Resources.Any(o => o.ResourceName == resourceName))
                    return true;
            }
            return false;
        }

        private void FillLinksNoRecursive(ResourceDescriptor descriptor, IResource resource, IDictionary<string, object> parameterValues, IEnumerable<HalRule> halRules)
        {
            // Add self
            resource.Links.Add(new Link
            {
                Rel = "self",
                Href = _uriResolver.Resovle(descriptor.ResourceUri, parameterValues)
            });

            var savedLinks = _resourceLinkPersistence.GetLinks(descriptor.ResourceName, null);

            foreach (var savedLink in savedLinks)
            {
                // if this link resource is not in the hal rule. continue;
                if (!IsResourceInRules(savedLink.DestinationResourceName, halRules))
                    continue;
                var targetResourceDescriptor = _resourceDescriptorProvider.GetDescriptor(savedLink.DestinationResourceName);

                AssertDescriptorNotNull(targetResourceDescriptor, savedLink.DestinationResourceName);
                parameterValues["src"] = descriptor.ResourceName;
                parameterValues["dest"] = savedLink.DestinationResourceName;
                parameterValues["rel"] = savedLink.Relation;

                var link = new Link
                {
                    Rel = savedLink.Relation,
                    Href = _uriResolver.Resovle(targetResourceDescriptor.ResourceUri, parameterValues)
                };

                resource.Links.Add(link);
            }

            AddImplicitLinks(descriptor, resource, parameterValues);
        }

        private void AddImplicitLinks(ResourceDescriptor descriptor, IResource resource, IDictionary<string, object> parameterValues)
        {
            if (descriptor.ImplicitLinkProvider != null)
            {
                var links = descriptor.ImplicitLinkProvider.GetImplicitLinks(_uriResolver, descriptor, parameterValues);
                if (links != null)
                {
                    foreach (var link in links)
                    {
                        resource.Links.Add(link);
                    }
                }
            }
        }

        private void AssertDescriptorNotNull(ResourceDescriptor descriptor, string resourceName)
        {
            if (descriptor == null)
                throw new InvalidOperationException("Cannot find resource descriptor for resource: " + resourceName + ", ensure the resource name is correct.");
        }
    }
}
