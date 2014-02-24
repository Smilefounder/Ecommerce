using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Dispatching
{
    public enum EventDispatchingPhase
    {
        OnEventRaised = 0,
        OnTransactionCommitted = 1
    }
}
