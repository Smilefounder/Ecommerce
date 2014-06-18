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
            var exists = repository.Query()
                                   .ByEvent(eventType)
                                   .Where(x => x.Type == RuleType.Always)
                                   .Any();

            if (!exists)
            {
                repository.Insert(new ActivityRule(eventType, RuleType.Always));
            }
        }
    }
}
