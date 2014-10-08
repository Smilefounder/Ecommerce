using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    public class ProductUnpublished : IProductEvent
    {
        [Reference(typeof(Product))]
        public int ProductId { get; set; }

        public ProductUnpublished() { }

        public ProductUnpublished(Product product)
        {
            ProductId = product.Id;
        }
    }
}
