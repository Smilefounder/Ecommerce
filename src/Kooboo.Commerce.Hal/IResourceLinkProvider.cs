using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Hal
{
    public interface IResourceLinkProvider
    {
        IEnumerable<ResourceLink> GetLinks(string resourceName);
    }
}
