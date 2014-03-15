using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public static class AttachedActivitiesExtensions
    {
        public static IEnumerable<AttachedActivity> WhereEnabled(this IEnumerable<AttachedActivity> activities)
        {
            return activities.Where(x => x.IsEnabled);
        }

        public static IEnumerable<AttachedActivity> SortByExecutionOrder(this IEnumerable<AttachedActivity> activities)
        {
            return activities.OrderByDescending(x => x.Priority)
                             .ThenBy(x => x.Id);
        }
    }
}
