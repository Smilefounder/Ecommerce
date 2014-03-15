using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities.Events
{
    public class ActivityAttached : IEvent
    {
        public ActivityRule Rule { get; private set; }

        public AttachedActivity Activity { get; private set; }

        public ActivityAttached(ActivityRule rule, AttachedActivity activity)
        {
            Rule = rule;
            Activity = activity;
        }
    }
}
