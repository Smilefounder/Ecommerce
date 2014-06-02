using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [Event(Order = 300)]
    public class ProductTypeEnabled : ProductTypeEventBase
    {
        protected ProductTypeEnabled() { }

        public ProductTypeEnabled(ProductType productType)
            : base(productType)
        {
        }
    }
}
