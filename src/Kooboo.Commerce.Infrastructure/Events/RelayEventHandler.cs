using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    public class RelayEventHandler<TEvent> : IHandle<TEvent>
        where TEvent : IEvent
    {
        private Action<TEvent, EventContext> _action;

        public RelayEventHandler(Action<TEvent, EventContext> action)
        {
            Require.NotNull(action, "action");
            _action = action;
        }

        public void Handle(TEvent @event, EventContext context)
        {
            _action(@event, context);
        }
    }
}
