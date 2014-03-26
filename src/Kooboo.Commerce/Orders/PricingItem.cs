using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders
{
    public class PricingItem
    {
        public int Id { get; set; }

        public ProductPrice ProductPrice { get; set; }

        [Parameter]
        public int Quantity { get; set; }

        public decimal Discount { get; set; }

        public decimal Subtotal
        {
            get
            {
                return Quantity * ProductPrice.PurchasePrice;
            }
        }

        public PricingItem(int id, ProductPrice price, int quantity)
        {
            Require.NotNull(price, "price");

            Id = id;
            ProductPrice = price;
            Quantity = quantity;
        }
    }
}
