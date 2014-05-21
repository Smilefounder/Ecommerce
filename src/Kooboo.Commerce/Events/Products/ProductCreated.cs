using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Serializable]
    public class ProductCreated : DomainEvent, IProductEvent
    {
        [ConditionParameter]
        public int ProductId { get; set; }

        public ProductCreated() { }

        public ProductCreated(Product product)
        {
            ProductId = product.Id;
        }
    }
}
