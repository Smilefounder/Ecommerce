using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [Event(Order = 400, ShortName = "Disabled")]
    public class ProductTypeDisabled : BusinessEvent, IProductTypeEvent
    {
        [Reference(typeof(ProductType))]
        public int ProductTypeId { get; set; }

        protected ProductTypeDisabled() { }

        public ProductTypeDisabled(ProductType productType)
        {
            ProductTypeId = productType.Id;
        }
    }
}
