using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Carts
{
    [ActivityEvent(Order = 400)]
    public class CartItemRemoved : Event, ICartEvent
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        [Reference(typeof(ProductPrice))]
        public int ProductPriceId { get; set; }

        [Param]
        public int Quantity { get; set; }

        protected CartItemRemoved() { }

        public CartItemRemoved(ShoppingCart cart, ShoppingCartItem item)
        {
            CartId = cart.Id;
            ItemId = item.Id;
            ProductId = item.ProductPrice.ProductId;
            ProductPriceId = item.ProductPrice.Id;
            Quantity = item.Quantity;
        }
    }
}
