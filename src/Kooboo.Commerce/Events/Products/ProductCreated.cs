using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [ActivityEvent(Order = 200)]
    public class ProductCreated : Event, IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        protected ProductCreated() { }

        public ProductCreated(Product product)
        {
            ProductId = product.Id;
        }
    }
}
