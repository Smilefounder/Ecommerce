using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [ActivityEvent(Order = 300)]
    public class ProductVariantCreated : Event, IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        [Reference(typeof(ProductVariant))]
        public int ProductVariantId { get; set; }

        protected ProductVariantCreated() { }

        public ProductVariantCreated(Product product, ProductVariant variant)
        {
            ProductId = product.Id;
            ProductVariantId = variant.Id;
        }
    }
}
