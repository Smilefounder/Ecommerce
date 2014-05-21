using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    public static class EventTypeExtensions
    {
        public static IEnumerable<Type> WhereIsDomainEvent(this IEnumerable<Type> types)
        {
            return types.Where(t => typeof(DomainEvent).IsAssignableFrom(t));
        }
    }
}
