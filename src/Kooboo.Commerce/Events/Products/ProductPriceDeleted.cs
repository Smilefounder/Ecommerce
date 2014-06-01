using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Event(Order = 1000)]
    public class ProductPriceDeleted : DomainEvent, IProductEvent
    {
        public int ProductId { get; set; }
    }
}
