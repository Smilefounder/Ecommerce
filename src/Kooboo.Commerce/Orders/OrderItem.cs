using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.ShoppingCarts;

namespace Kooboo.Commerce.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        /// <summary>
        /// The item that user really buy. One product can have multiple variants. Like one type of shop has multiple color or size, comsumer buys one of the variants.
        /// </summary>
        public int ProductPriceId { get; set; }

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

        public virtual ProductPrice ProductPrice { get; set; }

        public static OrderItem CreateFromCartItem(ShoppingCartItem cartItem, decimal finalUnitPrice)
        {
            return new OrderItem
            {
                ProductPriceId = cartItem.ProductPrice.Id,
                ProductPrice = cartItem.ProductPrice,
                ProductName = cartItem.ProductPrice.Name,
                SKU = cartItem.ProductPrice.Sku,
                UnitPrice = finalUnitPrice,
                Quantity = cartItem.Quantity
            };
        }
    }
}
