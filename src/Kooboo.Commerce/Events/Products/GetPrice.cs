using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [ActivityEvent(Order = 100)]
    public class GetPrice : Event, IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; private set; }

        [Reference(typeof(ProductPrice))]
        public int ProductPriceId { get; private set; }

        [Reference(Prefix = "")]
        public ShoppingContext ShoppingContext { get; private set; }

        [Param]
        public decimal OriginalUnitPrice { get; private set; }

        public decimal FinalUnitPrice { get; set; }

        private GetPrice() { }

        public GetPrice(int productId, int productPriceId, decimal originalUnitPrice, ShoppingContext shoppingContext)
        {
            ProductId = productId;
            ProductPriceId = productPriceId;
            OriginalUnitPrice = originalUnitPrice;
            FinalUnitPrice = originalUnitPrice;
            ShoppingContext = shoppingContext;
        }
    }
}
