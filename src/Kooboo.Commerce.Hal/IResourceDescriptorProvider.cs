using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL
{
    public interface IResourceDescriptorProvider
    {
        IEnumerable<ResourceDescriptor> GetAllDescriptors();

        ResourceDescriptor GetDescriptor(string resourceName);

        IEnumerable<ResourceDescriptor> GetLinkableResources(string resourceName);
    }
}
