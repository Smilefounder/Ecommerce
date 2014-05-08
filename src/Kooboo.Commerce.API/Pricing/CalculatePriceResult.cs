using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Pricing
{
    public class CalculatePriceResult
    {
        public IList<CalculateItemPriceResult> Items { get; set; }

        public PriceWithDiscount ShippingCost { get; set; }

        public PriceWithDiscount PaymentMethodCost { get; set; }

        public PriceWithDiscount Tax { get; set; }

        public PriceWithDiscount Subtotal { get; set; }

        public decimal Total { get; set; }

        public CalculatePriceResult()
        {
            Items = new List<CalculateItemPriceResult>();
        }
    }
}
