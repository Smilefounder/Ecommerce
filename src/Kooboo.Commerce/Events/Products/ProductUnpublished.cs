using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Event(Order = 400)]
    public class ProductUnpublished : DomainEvent, IProductEvent
    {
        public int ProductId { get; set; }

        protected ProductUnpublished() { }

        public ProductUnpublished(Product product)
        {
            ProductId = product.Id;
        }
    }
}
