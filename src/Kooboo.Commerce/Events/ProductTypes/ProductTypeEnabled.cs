using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [ActivityEvent(Order = 100)]
    public class ProductTypeEnabled : Event, IProductTypeEvent
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
