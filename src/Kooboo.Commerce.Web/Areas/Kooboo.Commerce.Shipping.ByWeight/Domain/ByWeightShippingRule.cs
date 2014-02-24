using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.ByWeight.Domain
{
    public class ByWeightShippingRule
    {
        public int Id { get; set; }

        public decimal FromWeight { get; set; }

        public decimal ToWeight { get; set; }

        public decimal ShippingPrice { get; set; }

        public ShippingPriceUnit PriceUnit { get; set; }

        public int ShippingMethodId { get; set; }
    }
}