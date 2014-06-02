using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [Event(Order = 500)]
    public class ProductTypeDeleted : DomainEvent, IProductTypeEvent
    {
        [Param]
        public int ProductTypeId { get; set; }

        [Param]
        public string ProductTypeName { get; set; }

        protected ProductTypeDeleted() { }

        public ProductTypeDeleted(ProductType productType)
        {
            ProductTypeId = productType.Id;
            ProductTypeName = productType.Name;
        }
    }
}
