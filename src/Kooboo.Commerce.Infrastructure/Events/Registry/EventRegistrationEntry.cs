using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Registry
{
    class EventRegistrationEntry : IComparable<EventRegistrationEntry>
    {
        public Type EventType { get; private set; }

        public int Order { get; private set; }

        public EventRegistrationEntry(Type eventType, int order)
        {
            EventType = eventType;
            Order = order;
        }

        public int CompareTo(EventRegistrationEntry other)
        {
            return Order.CompareTo(other.Order);
        }
    }
}
