using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    [Category("Product Types", Order = 250)]
    public interface IProductTypeEvent : IDomainEvent
    {
    }
}
