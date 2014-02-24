using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Events
{
    public class CommerceInstanceDeleted : IEvent
    {
        public CommerceInstanceMetadata Metadata { get; private set; }

        public CommerceInstanceDeleted(CommerceInstanceMetadata metadata)
        {
            Metadata = metadata;
        }
    }
}
