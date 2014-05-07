using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public interface IPropertyResourceProvider
    {
        string Name { get; }

        string Description { get; }

        IEnumerable<PropertyResource> GetPropertyResources(ResourceDescriptor descriptor, object entity);
    }
}
