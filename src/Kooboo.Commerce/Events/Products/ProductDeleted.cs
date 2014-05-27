using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Serializable]
    [Event(Category = EventCategories.Products, Order = 500)]
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
