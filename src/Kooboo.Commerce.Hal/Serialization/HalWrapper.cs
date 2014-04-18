using Kooboo.Commerce.HAL.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Controllers;

namespace Kooboo.Commerce.HAL.Serialization
{
    public class HalWrapper
    {
        private IUriResolver _uriResolver;
        private IResourceLinkPersistence _resourceLinkPersistence;
        private IResourceDescriptorProvider _resourceDescriptorProvider;

        public HalWrapper(
            IResourceDescriptorProvider resourceDescriptorProvider,
            IResourceLinkPersistence resourceLinkPersistence,
            IUriResolver uriResolver)
        {
            _resourceDescriptorProvider = resourceDescriptorProvider;
            _resourceLinkPersistence = resourceLinkPersistence;
            _uriResolver = uriResolver;
        }

        public Resource Wrap(string resourceName, object data, HttpControllerContext controllerContext, object parameterValues, Func<object, object> itemParameterValuesResolver = null)
        {
            var convertedParamValues = parameterValues.ToDictionary();
            Func<object, IDictionary<string, object>> convertedItemParamValuesResolver = null;
            if (itemParameterValuesResolver != null)
            {
                convertedItemParamValuesResolver = dataItem => itemParameterValuesResolver(dataItem).ToDictionary();
            }

            return Wrap(resourceName, data, controllerContext, convertedParamValues, convertedItemParamValuesResolver);
        }

        public Resource Wrap(string resourceName, object data, HttpControllerContext controllerContext, IDictionary<string, object> parameterValues, Func<object, IDictionary<string, object>> itemParameterValuesResolver = null)
        {
            if (String.IsNullOrEmpty(resourceName))
                throw new ArgumentException("Resource name is required.", "resourceName");

            if (data == null)
                throw new ArgumentNullException("data");

            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            var resource = new Resource();
            var descriptor = _resourceDescriptorProvider.GetDescriptor(resourceName);

            if (descriptor.IsListResource)
            {
                if (!(data is IEnumerable))
                    throw new InvalidOperationException("Resource '" + resourceName + "' is defined as a list resource, but the data passed in is not a collection.");

                var items = data as IEnumerable;
                var itemDescriptor = _resourceDescriptorProvider.GetDescriptor(descriptor.ItemResourceName);
                var wrappedItems = new List<Resource>();

                foreach (var item in items.OfType<object>())
                {
                    var itemParameters = itemParameterValuesResolver == null ? new Dictionary<string, object>() : itemParameterValuesResolver(item);
                    var wrappedItem = Wrap(itemDescriptor.ResourceName, item, controllerContext, itemParameters, itemParameterValuesResolver);
                    wrappedItems.Add(wrappedItem);
                }

                resource.Data = wrappedItems;
            }
            else
            {
                resource.Data = data;
            }

            // Add self
            resource.Links.Add(new Link
            {
                Rel = "self",
                Href = descriptor.ResourceUri// _uriResolver.Resovle(descriptor.ResourceUri, new Dictionary<string,object>(parameterValues))
            });

            var savedLinks = _resourceLinkPersistence.GetLinks(descriptor.ResourceName);

            foreach (var savedLink in savedLinks)
            {
                var targetResourceDescriptor = _resourceDescriptorProvider.GetDescriptor(savedLink.DestinationResourceName);
                if (targetResourceDescriptor == null)
                    throw new InvalidOperationException("Cannot find resource descriptor for resource: " + savedLink.DestinationResourceName + ", ensure the resource name is correct.");

                var link = new Link
                {
                    Rel = savedLink.Relation,
                    Href = targetResourceDescriptor.ResourceUri //_uriResolver.Resovle(targetResourceDescriptor.ResourceUri, new Dictionary<string, object>(parameterValues))
                };

                resource.Links.Add(link);
            }

            return resource;
        }
    }
}
