using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Event(Order = 500, ShortName = "Updated")]
    public class ProductUpdated : BusinessEvent, IProductEvent
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
