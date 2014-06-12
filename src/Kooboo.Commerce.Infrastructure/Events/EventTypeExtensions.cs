using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    public static class EventTypeExtensions
    {
        public static bool IsBusinessEvent(this Type type)
        {
            return typeof(BusinessEvent).IsAssignableFrom(type);
        }
    }
}
