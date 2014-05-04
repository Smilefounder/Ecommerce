using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Serializable]
    public class ProductDeleted : Event, IProductEvent
    {
        [ConditionParameter]
        public int ProductId { get; set; }

        public ProductDeleted() { }

        public ProductDeleted(Product product)
        {
            ProductId = product.Id;
        }
    }
}
