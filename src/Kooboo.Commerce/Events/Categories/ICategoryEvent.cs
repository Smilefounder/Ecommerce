using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Categories
{
    [Category("Categories", Order = 200)]
    public interface ICategoryEvent : IBusinessEvent
    {
        int CategoryId { get; }
    }
}
