using Kooboo.Commerce.Data;
using Kooboo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public static class ActivityRuleRepositoryExtensions
    {
        public static void EnsureAlwaysRule(this IRepository<ActivityRule> repository, Type eventType)
        {
            var eventTypeName = eventType.AssemblyQualifiedNameWithoutVersion();
            var exists = repository.Query()
                                   .Where(x => x.EventType == eventTypeName && x.Type == RuleType.Always)
                                   .Any();

            if (!exists)
            {
                repository.Insert(ActivityRule.Create(eventType, String.Empty, RuleType.Always));
            }
        }
    }
}
