using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Events
{
    public class CommerceInstanceCreating : IEvent
    {
        public CommerceInstanceMetadata Metadata { get; private set; }

        public CommerceInstanceCreating(CommerceInstanceMetadata metadata)
        {
            Require.NotNull(metadata, "metadata");
            Metadata = metadata;
        }
    }
}
