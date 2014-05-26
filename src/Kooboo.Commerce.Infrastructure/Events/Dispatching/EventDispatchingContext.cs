using Kooboo.Commerce.Data;
using System;

namespace Kooboo.Commerce.Events.Dispatching
{
    public class EventDispatchingContext
    {
        public EventDispatchingPhase Phase { get; private set; }

        public EventTrackingScope EventTrackingContext { get; private set; }

        public EventDispatchingContext(EventDispatchingPhase phase, EventTrackingScope eventTrackingContext)
        {
            Phase = phase;
            EventTrackingContext = eventTrackingContext;
        }
    }
}
