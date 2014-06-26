using Kooboo.Commerce.Products;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ProductTypes
{
    public interface IProductTypeEvent : IEvent
    {
        int ProductTypeId { get; }
    }
}
