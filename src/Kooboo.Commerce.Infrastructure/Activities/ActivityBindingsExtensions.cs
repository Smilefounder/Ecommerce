using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public static class ActivityBindingsExtensions
    {
        public static IQueryable<ActivityBinding> WhereBoundToEvent(this IQueryable<ActivityBinding> query, Type eventType)
        {
            var eventTypeName = eventType.GetVersionUnawareAssemblyQualifiedName();
            return query.Where(x => x.EventClrType == eventTypeName);
        }

        public static IQueryable<ActivityBinding> OrderByExecutionOrder(this IQueryable<ActivityBinding> query)
        {
            return query.OrderByDescending(x => x.Priority).ThenBy(x => x.Id);
        }
    }
}
