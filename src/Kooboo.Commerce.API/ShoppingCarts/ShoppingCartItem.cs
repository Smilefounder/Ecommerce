using Kooboo.Commerce.API.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.ShoppingCarts
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public ProductPrice ProductPrice { get; set; }

        public int Quantity { get; set; }
    }
}
