using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Carts;
using Kooboo.Commerce.Orders.Pricing;

namespace Kooboo.Commerce.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        /// <summary>
        /// The item that user really buy. One product can have multiple variants. Like one type of shop has multiple color or size, comsumer buys one of the variants.
        /// </summary>
        public int ProductVariantId { get; set; }

        /// <summary>
        /// redundant for query.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// redundant
        /// </summary>
        public string SKU { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Discount { get; set; }

        public decimal TaxCost { get; set; }

        public decimal Total { get; set; }

        public virtual Order Order { get; set; }

        public virtual ProductVariant ProductVariant { get; set; }

        public static OrderItem CreateFrom(ShoppingCartItem cartItem, decimal finalUnitPrice)
        {
            return new OrderItem
            {
                ProductVariantId = cartItem.ProductVariant.Id,
                ProductVariant = cartItem.ProductVariant,
                ProductName = cartItem.ProductVariant.Product.Name,
                SKU = cartItem.ProductVariant.Sku,
                UnitPrice = finalUnitPrice,
                Quantity = cartItem.Quantity
            };
        }
    }
}
