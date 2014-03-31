using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing
{
    /// <summary>
    /// Represents an item in a cart or order.
    /// So the price calculation pipeline is decoupled from the cart or order.
    /// </summary>
    public class PricingItem
    {
        public int Id { get; private set; }

        public ProductPrice ProductPrice { get; private set; }

        [Parameter]
        public int Quantity { get; private set; }

        public PriceWithDiscount Subtotal { get; set; }

        public PricingItem(int id, ProductPrice price, int quantity)
        {
            Require.NotNull(price, "price");

            Id = id;
            ProductPrice = price;
            Quantity = quantity;
            Subtotal = new PriceWithDiscount(price.RetailPrice * quantity);
        }
    }
}
