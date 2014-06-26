using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    public class GetPrice : Event, IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; private set; }

        [Reference(typeof(ProductPrice))]
        public int ProductPriceId { get; private set; }

        [Reference(Prefix = "")]
        public ShoppingContext ShoppingContext { get; private set; }

        [Param]
        public decimal OriginalPrice { get; private set; }

        public decimal FinalPrice { get; set; }

        private GetPrice() { }

        public GetPrice(int productId, int productPriceId, decimal originalPrice, ShoppingContext shoppingContext)
        {
            ProductId = productId;
            ProductPriceId = productPriceId;
            OriginalPrice = originalPrice;
            FinalPrice = originalPrice;
            ShoppingContext = shoppingContext;
        }
    }
}
