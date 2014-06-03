using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.HAL.Persistence;
using Kooboo.Commerce.API.HAL.Services;
using Kooboo.Commerce.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

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
            AddPropertyResourceLinks(descriptor, resource, context, parameterValues);
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
                    AddLinks(itemDescriptor.ResourceName, item, context, itemParamValues);
                }
            }
        }

        private IEnumerable<HalRule> GetHalRulesByHalContext(HalContext context)
        {
            if (_halRules == null)
                _halRules = _halRuleService.Query().ToArray();
            return _halRules.Where(o => String.IsNullOrEmpty(o.ConditionsExpression) || _ruleEngine.CheckCondition(o.ConditionsExpression, context));
        }

        private bool IsResourceInRules(string resourceName, IEnumerable<HalRule> halRules)
        {
            if (halRules == null || halRules.Count() <= 0)
                return true;
            foreach (var rule in halRules)
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
                Href = _uriResolver.Resovle(BuildResourceUri(descriptor, null), parameterValues)
            });

            var savedLinks = _resourceLinkPersistence.GetLinks(descriptor.ResourceName);

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
                    Href = _uriResolver.Resovle(BuildResourceUri(targetResourceDescriptor, savedLink.Parameters), parameterValues)
                };

                resource.Links.Add(link);
            }

            AddImplicitLinks(descriptor, resource, parameterValues);
        }

        private string BuildResourceUri(ResourceDescriptor descriptor, IEnumerable<ResourceLinkParameter> parameterValues)
        {
            string url = descriptor.ResourceUri;
            if (descriptor.InputPramameters != null && descriptor.InputPramameters.Length > 0 && parameterValues != null && parameterValues.Count() > 0)
            {
                var paras = new List<string>();
                foreach (var resPara in descriptor.InputPramameters)
                {
                    var halPara = parameterValues.FirstOrDefault(o => o.Name.ToLower() == resPara.Name);
                    if (halPara != null)
                    {
                        var dotIndex = halPara.Name.IndexOf('.');
                        var paraName = dotIndex > 0 ? halPara.Name.Substring(dotIndex + 1) : halPara.Name;
                        dotIndex = halPara.Value == null ? -1 : halPara.Value.IndexOf('.');
                        var paraValue = dotIndex > 0 ? halPara.Value.Substring(dotIndex + 1) : halPara.Value;
                        if (!string.IsNullOrEmpty(paraValue))
                        {
                            if (halPara.UseFixedValue)
                                paras.Add(string.Format("{0}={1}", paraName, paraValue));
                            else
                                paras.Add(string.Format("{0}={{{1}}}", paraName, paraValue));
                        }
                    }
                }
                if(url.IndexOf('?') > 0)
                {
                    var oqs = HttpUtility.ParseQueryString(url.Substring(url.IndexOf('?') + 1));
                    var nqs = HttpUtility.ParseQueryString(string.Join("&", paras.ToArray()));
                    foreach(var k in oqs.AllKeys)
                    {
                        if(string.IsNullOrEmpty(k) || nqs.AllKeys.Contains(k))
                            continue;
                        nqs.Add(k, oqs[k]);
                    }
                    var qsStrings = nqs.AllKeys.Select(o => string.Format("{0}={1}", o, nqs[o]));
                    url = url.Substring(0, url.IndexOf('?')) + "?" + string.Join("&", qsStrings.ToArray());
                }
                else
                {
                    var qs = string.Join("&", paras.ToArray());
                    url = url + "?" + qs;
                }
            }
            return url;
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

        private IDictionary<string, object> BuildEntityParameters(ResourceDescriptor descriptor, object entity, PropertyInfo[] properties = null, IDictionary<string, object> parentResourceParameterValues = null)
        {
            if (properties == null)
                properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var typeName = entity.GetType().Name;
            IDictionary<string, object> propParameters = new Dictionary<string, object>();
            if (descriptor.InputPramameters != null && descriptor.InputPramameters.Length > 0)
            {
                foreach (var ipara in descriptor.InputPramameters)
                {
                    var prop = properties.FirstOrDefault(o => HalParameter.NormailizeParameterName(typeName, o.Name) == ipara.Name.ToLower());
                    if (prop == null)
                        continue;
                    propParameters.Add(prop.Name.ToLower(), prop.GetValue(entity, null));
                }
            }
            if (parentResourceParameterValues != null && parentResourceParameterValues.Count > 0)
            {
                if(parentResourceParameterValues.ContainsKey("instance"))
                {
                    propParameters.Add("instance", parentResourceParameterValues["instance"]);
                }
                //foreach(var kvp in parentResourceParameterValues)
                //{
                //    if (propParameters.ContainsKey(kvp.Key))
                //        continue;
                //    propParameters.Add(kvp.Key, kvp.Value);
                //}
            }
            return propParameters;
        }

        public void AddPropertyResourceLinks(ResourceDescriptor descriptor, object entity, HalContext context, IDictionary<string, object> parentResourceParameterValues = null)
        {
            if (descriptor.PropertyResourceProvider != null)
            {
                var propResources = descriptor.PropertyResourceProvider.GetPropertyResources(descriptor, entity);
                if (propResources != null)
                {
                    foreach (var propResource in propResources)
                    {
                        if (propResource.ResourceNames == null || propResource.ResourceNames.Count() <= 0)
                            continue;
                        var properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var resName in propResource.ResourceNames)
                        {
                            var propRes = _resourceDescriptorProvider.GetDescriptor(resName);
                            if (propRes == null)
                                continue;
                            if (propResource.IsEnumerable)
                            {
                                foreach (var val in (IEnumerable)propResource.Value)
                                {
                                    // directly pass properties to the method to avoid calling reflection in the loop.
                                    var parameterValues = BuildEntityParameters(descriptor, val, properties, parentResourceParameterValues);
                                    AddLinks(resName, val as IItemResource, context, parameterValues);
                                }
                            }
                            else
                            {
                                var parameterValues = BuildEntityParameters(descriptor, entity, properties, parentResourceParameterValues);
                                AddLinks(resName, propResource.Value as IItemResource, context, parameterValues);
                            }
                        }
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
