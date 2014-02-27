using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Products
{
    public interface IQueryProductListEvent : IEvent
    {
        IEnumerable<Product> Result { get; set; }
    }
}
