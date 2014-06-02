using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [Event(Order = 500)]
    public class ProductTypeDeleted : ProductTypeEventBase
    {
        protected ProductTypeDeleted() { }

        public ProductTypeDeleted(ProductType productType)
            : base(productType)
        {
        }
    }
}
