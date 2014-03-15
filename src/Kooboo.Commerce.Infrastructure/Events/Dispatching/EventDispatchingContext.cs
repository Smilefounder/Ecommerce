using Kooboo.Commerce.Data;
using System;

namespace Kooboo.Commerce.Events.Dispatching
{
    public class EventDispatchingContext
    {
        public EventDispatchingPhase Phase { get; private set; }

        public EventTrackingContext EventTrackingContext { get; private set; }

        public EventDispatchingContext(EventDispatchingPhase phase, EventTrackingContext eventTrackingContext)
        {
            Phase = phase;
            EventTrackingContext = eventTrackingContext;
        }
    }
}
