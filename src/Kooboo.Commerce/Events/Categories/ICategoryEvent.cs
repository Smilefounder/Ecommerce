using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Categories
{
    public interface ICategoryEvent : IEvent
    {
        int CategoryId { get; }
    }
}
