using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Events
{
    public class CommerceInstanceDeleting : IEvent
    {
        public CommerceInstanceMetadata Metadata { get; private set; }

        public CommerceInstanceDeleting(CommerceInstanceMetadata metadata)
        {
            Metadata = metadata;
        }
    }
}
