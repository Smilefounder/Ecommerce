using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL
{
    public interface IUriResolver
    {
        ResourceDescriptor FindResource(string uri);
        string Resovle(string uriPattern, Dictionary<string, object> paras);
    }
}
