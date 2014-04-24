using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public interface IActivity
    {
        string Name { get; }

        string DisplayName { get; }

        bool AllowAsyncExecution { get; }

        bool CanBindTo(Type eventType);

        ActivityResult Execute(IEvent evnt, ActivityExecutionContext context);
    }

    public class ActivityExecutionContext
    {
        public ActivityRule Rule { get; private set; }

        public AttachedActivity AttachedActivity { get; private set; }

        public bool IsExecutedAsync { get; private set; }

        public ActivityExecutionContext(ActivityRule rule, AttachedActivity attachedActivity, bool isExecutedAsync)
        {
            Rule = rule;
            AttachedActivity = attachedActivity;
            IsExecutedAsync = isExecutedAsync;
        }
    }
}
