using System;

namespace Kooboo.Commerce.Events.Dispatching
{
    public interface IEventDispatcher
    {
        void Dispatch(IEvent evnt, EventDispatchingContext context);
    }
}
