using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Event(Order = 800)]
    public class ProductPricePublished : DomainEvent, IProductEvent
    {
        public int ProductId { get; set; }
    }
}
