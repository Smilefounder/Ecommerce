using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public interface IResourceDescriptorProvider
    {
        IEnumerable<ResourceDescriptor> GetAllDescriptors();

        ResourceDescriptor GetDescriptor(string resourceName);
    }
}
