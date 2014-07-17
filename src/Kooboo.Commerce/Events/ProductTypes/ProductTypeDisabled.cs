using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [ActivityEvent(Order = 200)]
    public class ProductTypeDisabled : Event, IProductTypeEvent
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
