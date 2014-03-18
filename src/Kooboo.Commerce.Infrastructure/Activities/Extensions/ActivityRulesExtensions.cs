using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Extensions;

namespace Kooboo.Commerce.Activities
{
    public static class ActivityRulesExtensions
    {
        public static bool HasAlwaysRule(this IEnumerable<ActivityRule> rules)
        {
            return rules.Any(x => x.Type == RuleType.Always);
        }

        public static IQueryable<ActivityRule> ByEvent(this IQueryable<ActivityRule> query, Type eventType)
        {
            var eventTypeName = eventType.AssemblyQualifiedNameWithoutVersion();
            return query.Where(x => x.EventType == eventTypeName)
                        .OrderBy(x => x.Type)
                        .ThenBy(x => x.Id);
        }
    }
}
