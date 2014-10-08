using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    public class ProductPublished : IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        public ProductPublished() { }

        public ProductPublished(Product product)
        {
            ProductId = product.Id;
        }
    }
}
