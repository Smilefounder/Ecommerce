using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [ActivityEvent(Order = 400)]
    public class ProductVariantUpdated : Event, IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        [Reference(typeof(ProductVariant))]
        public int ProductPriceId { get; set; }

        protected ProductVariantUpdated() { }

        public ProductVariantUpdated(Product product, ProductVariant price)
        {
            ProductId = product.Id;
            ProductPriceId = price.Id;
        }
    }
}
