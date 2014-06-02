using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [Event(Order = 200)]
    public class ProductTypeUpdated : ProductTypeEventBase
    {
        protected ProductTypeUpdated() { }

        public ProductTypeUpdated(ProductType productType)
            : base(productType)
        {
        }
    }
}
