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

        bool CanBindTo(Type eventType);

        ActivityResult Execute(IEvent evnt, ActivityExecutionContext context);
    }

    public class ActivityExecutionContext
    {
        public ActivityRule Rule { get; private set; }

        public AttachedActivity AttachedActivity { get; private set; }

        public ActivityExecutionContext(ActivityRule rule, AttachedActivity attachedActivity)
        {
            Rule = rule;
            AttachedActivity = attachedActivity;
        }
    }
}
