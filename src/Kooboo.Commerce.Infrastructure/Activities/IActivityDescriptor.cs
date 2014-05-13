using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public interface IActivityDescriptor
    {
        string Name { get; }

        Type ActivityType { get; }

        bool AllowAsyncExecution { get; }

        bool Configurable { get; }

        string ConfigViewVirtualPath { get; }

        bool CanBindTo(Type eventType);
    }
}
