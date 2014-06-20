using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public interface IHasCustomShippingRateProviderConfigEditor
    {
        string GetEditorVirtualPath(ShippingMethod shippingMethod);
    }
}
