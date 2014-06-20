using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight
{
    public enum ShippingPriceUnit
    {
        [Description("per KG")]
        PerKG = 0,

        [Description("per Order")]
        PerOrder = 1
    }
}