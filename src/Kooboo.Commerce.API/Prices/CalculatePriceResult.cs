using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Prices
{
    public class CalculatePriceResult
    {
        public IList<PricingItem> Items { get; set; }

        public decimal DiscountExItemDiscounts { get; set; }

        public decimal ShippingCost { get; set; }

        public decimal ShippingDiscount { get; set; }

        public decimal PaymentMethodCost { get; set; }

        public decimal PaymentMethodDiscount { get; set; }

        public decimal Tax { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

        public CalculatePriceResult()
        {
            Items = new List<PricingItem>();
        }
    }
}
