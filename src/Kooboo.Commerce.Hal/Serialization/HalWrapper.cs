using Kooboo.Commerce.HAL.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Resource Wrap(ResourceResponse response, Dictionary<string, object> paras)
        {
            var resource = new Resource
            {
                Name = response.ResourceDescriptor.ResourceName
            };

            if (response.ResourceDescriptor.IsListResource)
            {
                if (!(response.Data is IEnumerable))
                    throw new InvalidOperationException("Resource '" + resource.Name + "' is defined as a list resource, but the data passed in is not a collection.");

                var items = response.Data as IEnumerable;
                var itemDescriptor = _resourceDescriptorProvider.GetDescriptor(response.ResourceDescriptor.ItemResourceName);
                var wrappedItems = new List<Resource>();

                foreach (var item in items.OfType<object>())
                {
                    // TODO: This might be problematic,
                    //       Think about /brands?param=xxx list resource, 
                    //       the embedded item resource's self link is /brand/{id},
                    //       so how can I have {id} value in the 'paras' dictionary?
                    var wrappedItem = Wrap(new ResourceResponse(itemDescriptor.ResourceUri, itemDescriptor, item), paras);
                    wrappedItems.Add(wrappedItem);
                }

                resource.Data = wrappedItems;
            }
            else
            {
                resource.Data = response.Data;
            }

            // Add self
            resource.Links.Add(new Link
            {
                Rel = "self",
                Href = response.ResourceUri
            });

            var savedLinks = _resourceLinkPersistence.GetLinks(response.ResourceDescriptor.ResourceName);

            foreach (var savedLink in savedLinks)
            {
                var targetResourceDescriptor = _resourceDescriptorProvider.GetDescriptor(savedLink.DestinationResourceName);
                if (targetResourceDescriptor == null)
                    throw new InvalidOperationException("Cannot find resource descriptor for resource: " + savedLink.DestinationResourceName + ", ensure the resource name is correct.");

                var link = new Link
                {
                    Rel = savedLink.Relation,
                    Href = targetResourceDescriptor.ResourceUri //_uriResolver.Resovle(targetResourceDescriptor.ResourceUri, paras)
                };

                resource.Links.Add(link);
            }

            return resource;
        }
    }
}
