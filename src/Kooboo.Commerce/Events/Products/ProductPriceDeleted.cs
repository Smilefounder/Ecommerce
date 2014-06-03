using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Event(Order = 1000)]
    public class ProductPriceDeleted : DomainEvent, IProductEvent
    {
        [Param]
        public int ProductId { get; set; }

        [Param]
        public int ProductPriceId { get; set; }

        protected ProductPriceDeleted() { }

        public ProductPriceDeleted(Product product, ProductPrice price)
        {
            ProductId = product.Id;
            ProductPriceId = price.Id;
        }
    }
}
