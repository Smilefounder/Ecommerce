using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using Kooboo.Commerce.Carts;
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

        [Reference(typeof(ProductVariant))]
        public int VariantId { get; private set; }

        [Reference(Prefix = "")]
        public ShoppingContext ShoppingContext { get; private set; }

        [Param]
        public decimal OriginalPrice { get; private set; }

        public decimal FinalPrice { get; set; }

        protected GetPrice() { }

        public GetPrice(int productId, int variantId, decimal originalPrice, ShoppingContext shoppingContext)
        {
            ProductId = productId;
            VariantId = variantId;
            OriginalPrice = originalPrice;
            FinalPrice = originalPrice;
            ShoppingContext = shoppingContext;
        }
    }
}
