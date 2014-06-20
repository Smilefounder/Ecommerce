using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight
{
    public class ByWeightShippingRule
    {
        public decimal FromWeight { get; set; }

        public decimal ToWeight { get; set; }

        public decimal ShippingPrice { get; set; }

        public ShippingPriceUnit PriceUnit { get; set; }
    }
}