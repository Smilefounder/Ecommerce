using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Hal
{
    public interface IResourceDescriptorProvider
    {
        IEnumerable<ResourceDescriptor> GetAllDescriptors();

        ResourceDescriptor GetDescriptorFor(string resourceName);
    }
}
