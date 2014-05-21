using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public class ShippingRateProviderEditor
    {
        public string VirtualPath { get; private set; }

        public ShippingRateProviderEditor(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");
            VirtualPath = virtualPath;
        }
    }
}
