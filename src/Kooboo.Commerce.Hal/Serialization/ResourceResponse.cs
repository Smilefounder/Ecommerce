using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.HAL.Serialization
{
    public class ResourceResponse
    {
        public string ResourceUri { get; set; }

        public ResourceDescriptor ResourceDescriptor { get; set; }

        public object Data { get; set; }

        public ResourceResponse(string resourceUri, ResourceDescriptor descriptor, object data)
        {
            ResourceUri = resourceUri;
            ResourceDescriptor = descriptor;
            Data = data;
        }
    }
}
