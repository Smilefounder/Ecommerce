using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Registry
{
    public class EventRegistrationEntry : IComparable<EventRegistrationEntry>
    {
        public Type EventType { get; private set; }

        public string Category { get; private set; }

        public string DisplayName { get; private set; }

        public int Order { get; private set; }

        public EventRegistrationEntry(Type eventType)
        {
            EventType = eventType;
            DisplayName = eventType.Name;

            var eventAttr = eventType.GetCustomAttribute<EventAttribute>(true);
            if (eventAttr != null)
            {
                Order = eventAttr.Order;
                if (!String.IsNullOrEmpty(eventAttr.DisplayName))
                {
                    DisplayName = eventAttr.DisplayName;
                }
                if (!String.IsNullOrEmpty(eventAttr.Category))
                {
                    Category = eventAttr.Category;
                }
            }
        }

        public EventRegistrationEntry(Type eventType, string displayName, string category, int order)
        {
            EventType = eventType;
            DisplayName = displayName;
            Category = category;
            Order = order;
        }

        public int CompareTo(EventRegistrationEntry other)
        {
            return Order.CompareTo(other.Order);
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
