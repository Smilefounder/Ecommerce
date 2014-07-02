using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Events
{
    public class CommerceInstanceCreated : Event
    {
        public CommerceInstanceSettings InstanceSettings { get; private set; }

        public CommerceInstanceCreated(CommerceInstanceSettings settings)
        {
            InstanceSettings = settings;
        }
    }
}
