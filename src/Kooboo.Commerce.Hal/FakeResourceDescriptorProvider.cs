using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL
{
    [Dependency(typeof(IResourceDescriptorProvider))]
    public class FakeResourceDescriptorProvider : IResourceDescriptorProvider
    {
        static readonly List<ResourceDescriptor> _descriptors = new List<ResourceDescriptor>();

        static FakeResourceDescriptorProvider()
        {
            var productDetail = new ResourceDescriptor("ProductDetail", "/product/{id}");
            _descriptors.Add(productDetail);

            var productList = new ResourceDescriptor("ProductList", "/products");
            _descriptors.Add(productList);

            var accessories = new ResourceDescriptor("ProductAccessories", "/product/{id}/accessories");
            _descriptors.Add(accessories);
        }

        public IEnumerable<ResourceDescriptor> GetAllDescriptors()
        {
            return _descriptors;
        }

        public ResourceDescriptor GetDescriptor(string resourceName)
        {
            return _descriptors.FirstOrDefault(x => x.ResourceName == resourceName);
        }
    }
}
