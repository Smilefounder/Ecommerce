using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Pricing
{
    public class PricingItem
    {
        public int Id { get; set; }

        public int ProductPriceId { get; set; }

        public int Quantity { get; set; }

        public PriceWithDiscount Subtotal { get; set; }
    }
}
