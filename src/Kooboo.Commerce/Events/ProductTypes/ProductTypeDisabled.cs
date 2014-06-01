using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [Event(Order = 400)]
    public class ProductTypeDisabled : DomainEvent, IProductTypeEvent
    {
    }
}
