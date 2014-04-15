using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL
{
    public interface IResourceLinkProvider
    {
        IEnumerable<ResourceLink> GetLinks(string resourceName);
    }
}
