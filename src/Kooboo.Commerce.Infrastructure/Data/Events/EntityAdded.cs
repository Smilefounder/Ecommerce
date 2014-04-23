using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Events
{
    public class EntityAdded : Event
    {
        public string CommerceName { get; private set; }

        public object Entity { get; private set; }

        public EntityAdded(string commerceName, object entity)
        {
            Require.NotNullOrEmpty(commerceName, "commerceName");
            Require.NotNull(entity, "entity");

            CommerceName = commerceName;
            Entity = entity;
        }
    }
}
