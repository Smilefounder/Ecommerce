using Kooboo.Commerce.API.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Pricing
{
    public class CalculatePriceRequest
    {
        public IList<PricingItem> Items { get; set; }

        public int? CustomerId { get; set; }

        public string CouponCode { get; set; }

        public int? PaymentMethodId { get; set; }

        public int? ShippingMethodId { get; set; }

        public CalculatePriceRequest()
        {
            Items = new List<PricingItem>();
        }
    }
}
