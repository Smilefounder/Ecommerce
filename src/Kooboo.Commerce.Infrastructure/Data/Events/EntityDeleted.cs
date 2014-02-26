using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Events
{
    public class EntityDeleted : IPersistentEvent
    {
        public object Entity { get; private set; }

        public EntityDeleted(object entity)
        {
            Entity = entity;
        }
    }
}
