using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL
{
    public class ResourceDescriptor
    {
        public string ResourceName { get; private set; }

        public string ResourceUri { get; private set; }

        public IList<ResourceParameter> InputParameters { get; private set; }

        public IList<ResourceParameter> OutputParameters { get; private set; }

        public ResourceDescriptor(string resourceName, string resourceUri)
        {
            ResourceName = resourceName;
            ResourceUri = resourceUri;
        }
    }
}
