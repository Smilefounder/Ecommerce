using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Carts
{
    public class CartItemRemoved : Event, ICartEvent
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        [Reference(typeof(ProductVariant))]
        public int ProductPriceId { get; set; }

        [Param]
        public int Quantity { get; set; }

        protected CartItemRemoved() { }

        public CartItemRemoved(ShoppingCart cart, ShoppingCartItem item)
        {
            CartId = cart.Id;
            ItemId = item.Id;
            ProductId = item.ProductVariant.ProductId;
            ProductPriceId = item.ProductVariant.Id;
            Quantity = item.Quantity;
        }
    }
}
