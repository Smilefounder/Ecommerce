using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Carts
{
    [ActivityEvent(Order = 200)]
    public class CartItemAdded : Event, ICartEvent
    {
        public int CartId { get; protected set; }

        public int ItemId { get; protected set; }

        [Reference(typeof(Product))]
        public int ProductId { get; protected set; }

        [Reference(typeof(ProductVariant))]
        public int ProductPriceId { get; protected set; }

        [Param]
        public int Quantity { get; protected set; }

        protected CartItemAdded() { }

        public CartItemAdded(ShoppingCart cart, ShoppingCartItem item)
        {
            CartId = cart.Id;
            ItemId = item.Id;
            ProductId = item.ProductVariant.ProductId;
            ProductPriceId = item.ProductVariant.Id;
            Quantity = item.Quantity;
        }
    }
}
