using Kooboo.Commerce.Data;
using System;

namespace Kooboo.Commerce.Events.Dispatching
{
    public class EventDispatchingContext
    {
        public EventDispatchingPhase Phase { get; private set; }

        public bool IsInEventTrackingScope { get; private set; }

        public EventDispatchingContext(EventDispatchingPhase phase, bool isInEventTrackingScope)
        {
            Phase = phase;
            IsInEventTrackingScope = isInEventTrackingScope;
        }
    }
}
