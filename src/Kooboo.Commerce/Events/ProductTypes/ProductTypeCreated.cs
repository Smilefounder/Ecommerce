using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [Event(Order = 100)]
    public class ProductTypeCreated : BusinessEvent, IProductTypeEvent
    {
        [Reference(typeof(ProductType))]
        public int ProductTypeId { get; set; }

        protected ProductTypeCreated() { }

        public ProductTypeCreated(ProductType productType)
        {
            ProductTypeId = productType.Id;
        }
    }
}
