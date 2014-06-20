using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShoppingCarts
{
    [Event(Order = 300, ShortName = "Item Quantity Changed")]
    public class CartItemQuantityChanged : BusinessEvent, IShoppingCartEvent
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        [Reference(typeof(ProductPrice))]
        public int ProductPriceId { get; set; }

        [Param]
        public int OldQuantity { get; set; }

        [Param]
        public int NewQuantity { get; set; }

        protected CartItemQuantityChanged() { }

        public CartItemQuantityChanged(ShoppingCart cart, ShoppingCartItem item, int oldQuantity)
        {
            CartId = cart.Id;
            ItemId = item.Id;
            ProductId = item.ProductPrice.ProductId;
            ProductPriceId = item.ProductPrice.Id;
            OldQuantity = oldQuantity;
            NewQuantity = item.Quantity;
        }
    }
}
