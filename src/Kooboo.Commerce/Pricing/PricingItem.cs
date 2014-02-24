using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Pricing
{
    public class PricingItem
    {
        public Product Product { get; set; }

        public ProductPrice Variation { get; set; }

        public int Quantity { get; set; }

        public decimal Subtotal
        {
            get
            {
                return Variation.PurchasePrice * Quantity;
            }
        }

        public decimal Total { get; set; }

        public PricingItem(Product product, ProductPrice variation)
        {
            Require.NotNull(product, "product");
            Require.NotNull(variation, "variation");

            Product = product;
            Variation = variation;
        }

        public void ApplyDiscount(decimal discount)
        {
            var newTotal = Total - discount;
            if (newTotal < 0)
            {
                newTotal = 0;
            }

            Total = newTotal;
        }
    }
}
