using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Events
{
    public class CommerceInstanceCreated : IEvent
    {
        public string InstanceName { get; private set; }

        public CommerceInstanceSettings Settings { get; private set; }

        public CommerceInstanceCreated(string instanceName, CommerceInstanceSettings settings)
        {
            InstanceName = instanceName;
            Settings = settings;
        }
    }
}
