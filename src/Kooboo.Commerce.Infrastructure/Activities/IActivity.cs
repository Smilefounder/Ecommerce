using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public interface IActivity
    {
        string Name { get; }

        string DisplayName { get; }

        bool CanBindTo(Type eventType);

        ActivityResponse Execute(IEvent evnt, ActivityBinding binding);
    }
}
