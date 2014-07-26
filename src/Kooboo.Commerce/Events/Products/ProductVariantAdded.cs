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
    public class ProductVariantAdded : Event, IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        [Reference(typeof(ProductVariant))]
        public int ProductPriceId { get; set; }

        protected ProductVariantAdded() { }

        public ProductVariantAdded(Product product, ProductVariant price)
        {
            ProductId = product.Id;
            ProductPriceId = price.Id;
        }
    }
}
