using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;

namespace Kooboo.Commerce.Carts
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }

        public virtual ProductVariant ProductVariant { get; set; }

        [Param]
        public int Quantity { get; set; }

        public ShoppingCartItem() { }

        public ShoppingCartItem(ProductVariant variant, int quantity, ShoppingCart cart)
        {
            ProductVariant = variant;
            Quantity = quantity;
            ShoppingCart = cart;
        }
    }
}