using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Carts
{
    public class CartItemQuantityChanged : ICartEvent
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        [Reference(typeof(ProductVariant))]
        public int ProductPriceId { get; set; }

        [Param]
        public int OldQuantity { get; set; }

        [Param]
        public int NewQuantity { get; set; }

        public CartItemQuantityChanged() { }

        public CartItemQuantityChanged(ShoppingCart cart, ShoppingCartItem item, int oldQuantity)
        {
            CartId = cart.Id;
            ItemId = item.Id;
            ProductId = item.ProductVariant.ProductId;
            ProductPriceId = item.ProductVariant.Id;
            OldQuantity = oldQuantity;
            NewQuantity = item.Quantity;
        }
    }
}
