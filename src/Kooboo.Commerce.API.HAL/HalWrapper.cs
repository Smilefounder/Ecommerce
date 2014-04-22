using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.HAL.Persistence;
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

        public HalWrapper(
            IResourceDescriptorProvider resourceDescriptorProvider,
            IResourceLinkPersistence resourceLinkPersistence,
            IUriResolver uriResolver,
            IEnumerable<IContextEnviromentProvider> environmentProviders)
        {
            _resourceDescriptorProvider = resourceDescriptorProvider;
            _resourceLinkPersistence = resourceLinkPersistence;
            _uriResolver = uriResolver;
            _environmentProviders = environmentProviders;
        }

        public void AddLinks(string resourceName, IItemResource resource, HalContext context, IDictionary<string, object> parameterValues)
        {
            if (String.IsNullOrEmpty(resourceName))
                throw new ArgumentException("Resource name is required.", "resourceName");

            if (resource == null)
                throw new ArgumentNullException("resource");

            var descriptor = _resourceDescriptorProvider.GetDescriptor(resourceName);
            AssertDescriptorNotNull(descriptor, resourceName);
            FillLinksNoRecursive(descriptor, resource, parameterValues, GetAvailableEnvironmentNames(context));
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

            var environmentNames = GetAvailableEnvironmentNames(context);

            FillLinksNoRecursive(descriptor, resource, parameterValues, environmentNames);

            var itemDescriptor = _resourceDescriptorProvider.GetDescriptor(descriptor.ItemResourceName);
            AssertDescriptorNotNull(itemDescriptor, descriptor.ItemResourceName);

            foreach (var item in resource)
            {
                var itemParamValues = itemParameterValuesResolver == null ? null : itemParameterValuesResolver(item);
                FillLinksNoRecursive(itemDescriptor, item, itemParamValues, environmentNames);
            }
        }

        private ISet<string> GetAvailableEnvironmentNames(HalContext context)
        {
            var providers = _environmentProviders.Where(x => x.IsContextRunInEnviroment(context)).ToList();
            return new HashSet<string>(providers.Select(x => x.Name));
        }

        private void FillLinksNoRecursive(ResourceDescriptor descriptor, IResource resource, IDictionary<string, object> parameterValues, ISet<string> environmentProviderNames)
        {
            // Add self
            resource.Links.Add(new Link
            {
                Rel = "self",
                Href = _uriResolver.Resovle(descriptor.ResourceUri, parameterValues)
            });

            var savedLinks = _resourceLinkPersistence.GetLinks(descriptor.ResourceName, environmentProviderNames);

            foreach (var savedLink in savedLinks)
            {
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
