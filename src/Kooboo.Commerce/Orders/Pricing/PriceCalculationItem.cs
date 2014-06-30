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
    public class PriceCalculationItem
    {
        public int ItemId { get; private set; }

        [Reference(typeof(Product))]
        public int ProductId { get; private set; }

        [Reference(typeof(ProductPrice))]
        public int ProductPriceId { get; private set; }

        [Param]
        public decimal UnitPrice { get; private set; }

        [Param]
        public int Quantity { get; private set; }

        public decimal Subtotal { get; private set; }

        public decimal Discount { get; set; }

        public PriceCalculationItem(int itemId, int productId, int productPriceId, decimal unitPrice, int quantity)
        {
            ItemId = itemId;
            ProductId = productId;
            ProductPriceId = productPriceId;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Subtotal = UnitPrice * quantity;
        }
    }
}
