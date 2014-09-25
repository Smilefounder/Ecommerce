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
        private Action<TEvent, CommerceInstance> _action;

        public RelayEventHandler(Action<TEvent, CommerceInstance> action)
        {
            Require.NotNull(action, "action");
            _action = action;
        }

        public void Handle(TEvent @event, CommerceInstance instance)
        {
            _action(@event, instance);
        }
    }
}
