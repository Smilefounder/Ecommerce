using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [Event(Order = 300, ShortName = "Enabled")]
    public class ProductTypeEnabled : BusinessEvent, IProductTypeEvent
    {
        [Reference(typeof(ProductType))]
        public int ProductTypeId { get; set; }

        protected ProductTypeEnabled() { }

        public ProductTypeEnabled(ProductType productType)
        {
            ProductTypeId = productType.Id;
        }
    }
}
