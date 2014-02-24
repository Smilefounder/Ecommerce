using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.ShoppingCarts
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }

        public virtual ProductPrice ProductPrice { get; set; }

        public int Quantity { get; set; }

        protected ShoppingCartItem() { }

        public ShoppingCartItem(ProductPrice variation, int quantity, ShoppingCart cart)
        {
            ProductPrice = variation;
            Quantity = quantity;
            ShoppingCart = cart;
        }
    }
}