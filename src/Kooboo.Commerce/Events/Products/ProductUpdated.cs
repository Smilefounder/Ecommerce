using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [ActivityEvent(Order = 600)]
    public class ProductUpdated : Event, IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        protected ProductUpdated() { }

        public ProductUpdated(Product product)
        {
            ProductId = product.Id;
        }
    }
}
