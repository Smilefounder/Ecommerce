using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    public interface IHandle<in TEvent>
        where TEvent: IEvent
    {
        void Handle(TEvent @event, CommerceInstance instance);
    }
}
