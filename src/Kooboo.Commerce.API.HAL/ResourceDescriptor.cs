using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    public class ResourceDescriptor
    {
        public ResourceName ResourceName { get; set; }

        public string ResourceUri { get; set; }

        public bool IsListResource { get; set; }

        private string itemResourceName;
        public string ItemResourceName
        {
            get { return itemResourceName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    IsListResource = true;
                }
                itemResourceName = value;
            }
        }

        public ResourceDescriptor()
        {
        }

        public ResourceDescriptor(string resourceName, string resourceUri, bool isListResource = false, string itemResourceName = null)
        {
            ResourceName = resourceName;
            ResourceUri = resourceUri;
            IsListResource = isListResource;
            ItemResourceName = itemResourceName;
        }

        public IImplicitLinkProvider ImplicitLinkProvider { get; set; }

        public HalParameter[] InputPramameters { get; set; }
        public HalParameter[] OutputParameters { get; set; }
    }
}
