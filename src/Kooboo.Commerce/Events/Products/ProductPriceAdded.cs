using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Event(Order = 600)]
    public class ProductPriceAdded : DomainEvent, IProductEvent
    {
        public int ProductId { get; set; }
    }
}
