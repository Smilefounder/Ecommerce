using Kooboo.Commerce.Events.Dispatching;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.Commerce.Events
{
    public static class Event
    {
        public static void Raise<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            Require.NotNull(@event, "event");

            var dispatcher = EngineContext.Current.Resolve<IEventDispatcher>();
            if (dispatcher == null)
                throw new InvalidOperationException("Cannot resolve event dispatcher. Ensure event dispatcher is registered.");

            var eventTrackingContext = EventTrackingContext.Current;
            dispatcher.Dispatch(@event, new EventDispatchingContext(EventDispatchingPhase.OnEventRaised, eventTrackingContext));

            if (eventTrackingContext != null)
            {
                eventTrackingContext.AppendEvent(@event);
            }
        }
    }
}
