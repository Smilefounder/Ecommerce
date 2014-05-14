using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public static class AttachedActivityInfoExtensions
    {
        public static AttachedActivityInfo Find(this IEnumerable<AttachedActivityInfo> activities, int attachedActivityId)
        {
            return activities.FirstOrDefault(x => x.Id == attachedActivityId);
        }

        public static IEnumerable<AttachedActivityInfo> WhereEnabled(this IEnumerable<AttachedActivityInfo> activities)
        {
            return activities.Where(x => x.IsEnabled);
        }

        public static IEnumerable<AttachedActivityInfo> OrderByExecutionOrder(this IEnumerable<AttachedActivityInfo> activities)
        {
            return activities.OrderByDescending(x => x.Priority)
                             .ThenBy(x => x.Id);
        }
    }
}
