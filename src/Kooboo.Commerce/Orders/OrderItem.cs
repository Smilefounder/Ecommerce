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

        public int ProductVariantId { get; set; }

        public string Sku { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }

        public virtual Order Order { get; set; }

        public virtual ProductVariant ProductVariant { get; set; }

        public static OrderItem CreateFrom(ShoppingCartItem cartItem, decimal finalUnitPrice)
        {
            return new OrderItem
            {
                ProductVariantId = cartItem.ProductVariant.Id,
                ProductVariant = cartItem.ProductVariant,
                Sku = cartItem.ProductVariant.Sku,
                UnitPrice = finalUnitPrice,
                Quantity = cartItem.Quantity
            };
        }
    }
}
