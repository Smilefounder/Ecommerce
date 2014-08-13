using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Carts
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        [OptionalInclude]
        public Product Product { get; set; }

        [OptionalInclude]
        public ProductVariant ProductVariant { get; set; }

        public int Quantity { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }
    }
}
