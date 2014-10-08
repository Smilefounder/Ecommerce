using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    public interface IHandle<in TEvent>
        where TEvent: IEvent
    {
        void Handle(TEvent @event, EventContext context);
    }

    public class EventContext
    {
        public CommerceInstance Instance { get; private set; }

        public DateTime TimestampUtc { get; private set; }

        public EventContext()
            : this(null)
        {
        }

        public EventContext(CommerceInstance instance)
        {
            Instance = instance;
            TimestampUtc = DateTime.UtcNow;
        }
    }
}
