using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Events
{
    public class EntityCreated : IPersistentEvent
    {
        public object Entity { get; private set; }

        public EntityCreated(object entity)
        {
            Entity = entity;
        }
    }
}
