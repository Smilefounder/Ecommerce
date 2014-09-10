using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    public class ProductPublished : Event, IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        protected ProductPublished() { }

        public ProductPublished(Product product)
        {
            ProductId = product.Id;
        }
    }
}
