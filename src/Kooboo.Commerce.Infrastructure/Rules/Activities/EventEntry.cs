using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Activities
{
    public class EventEntry
    {
        public Type EventType { get; private set; }

        public string EventName
        {
            get
            {
                return EventType.Name;
            }
        }

        public string Category { get; private set; }

        public string DisplayName { get; private set; }

        public string ShortName { get; private set; }

        public EventEntry(string category, string displayName, string shortName, Type eventType)
        {
            Require.NotNull(eventType, "eventType");

            Category = category;
            DisplayName = displayName;
            ShortName = shortName;
            EventType = eventType;
        }
    }
}
