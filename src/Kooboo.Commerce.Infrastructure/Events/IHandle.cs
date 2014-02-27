using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    public interface IHandles<TEvent>
        where TEvent: IEvent
    {
        void Handle(TEvent @event, EventDispatchingContext context);
    }
}
