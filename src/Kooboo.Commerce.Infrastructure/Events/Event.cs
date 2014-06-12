using Kooboo.Commerce.Events.Dispatching;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.Commerce.Events
{
    [Serializable]
    public abstract class Event : IEvent
    {
        public DateTime TimestampUtc { get; set; }

        protected Event()
        {
            TimestampUtc = DateTime.UtcNow;
        }

        public static void Raise(IEvent @event)
        {
            Require.NotNull(@event, "event");

            var dispatcher = EngineContext.Current.Resolve<IEventDispatcher>();
            if (dispatcher == null)
                throw new InvalidOperationException("Cannot resolve event dispatcher. Ensure event dispatcher is registered.");

            dispatcher.Dispatch(@event);
        }
    }
}
