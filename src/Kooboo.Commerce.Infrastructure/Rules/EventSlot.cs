using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class EventSlot
    {
        public string EventName { get; private set; }

        public IList<RuleBase> Rules { get; private set; }

        public EventSlot(string eventName)
        {
            Require.NotNullOrEmpty(eventName, "eventName");

            EventName = eventName;
            Rules = new List<RuleBase>();
        }

        public EventSlot(string eventName, IEnumerable<RuleBase> rules)
        {
            Require.NotNullOrEmpty(eventName, "eventName");
            Require.NotNull(rules, "rules");

            EventName = eventName;
            Rules = rules.ToList();
        }

        public override string ToString()
        {
            return EventName;
        }
    }
}
