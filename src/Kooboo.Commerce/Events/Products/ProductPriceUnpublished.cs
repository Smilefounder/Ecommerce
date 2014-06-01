using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    [Event(Order = 900)]
    public class ProductPriceUnpublished : DomainEvent, IProductEvent
    {
        public int ProductId { get; set; }
    }
}
