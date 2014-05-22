using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Serializable]
    public class ProductDeleted : DomainEvent, IProductEvent
    {
        [Param]
        public int ProductId { get; set; }

        public ProductDeleted() { }

        public ProductDeleted(Product product)
        {
            ProductId = product.Id;
        }
    }
}
