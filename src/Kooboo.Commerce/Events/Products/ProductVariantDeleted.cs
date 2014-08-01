using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [ActivityEvent(Order = 500)]
    public class ProductVariantDeleted : Event, IProductEvent
    {
        [Param]
        public int ProductId { get; set; }

        [Param]
        public int ProductVariantId { get; set; }

        protected ProductVariantDeleted() { }

        public ProductVariantDeleted(Product product, ProductVariant variant)
        {
            ProductId = product.Id;
            ProductVariantId = variant.Id;
        }
    }
}
