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
        public int ItemId { get; private set; }

        [Param]
        public int ProductId { get; private set; }

        [Param]
        public int ProductPriceId { get; private set; }

        [Param]
        public decimal RetailPrice { get; private set; }

        [Param]
        public int Quantity { get; private set; }

        public PriceWithDiscount Subtotal { get; set; }

        public PricingItem(int itemId, int productId, int productPriceId, decimal retailPrice, int quantity)
        {
            ItemId = itemId;
            ProductId = productId;
            ProductPriceId = productPriceId;
            RetailPrice = retailPrice;
            Quantity = quantity;
            Subtotal = new PriceWithDiscount(RetailPrice * quantity);
        }
    }
}
