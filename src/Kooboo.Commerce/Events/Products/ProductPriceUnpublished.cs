using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Event(Order = 900)]
    public class ProductPriceUnpublished : DomainEvent, IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        [Reference(typeof(ProductPrice))]
        public int ProductPriceId { get; set; }

        protected ProductPriceUnpublished() { }

        public ProductPriceUnpublished(Product product, ProductPrice price)
        {
            ProductId = product.Id;
            ProductPriceId = price.Id;
        }
    }
}
