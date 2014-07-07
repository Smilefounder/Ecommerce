using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Globalization
{
    public class EntityKey
    {
        public string EntityType { get; private set; }

        public object[] Values { get; private set; }

        public EntityKey(string entityType, object[] values)
        {
            Require.NotNullOrEmpty(entityType, "entityType");
            Require.NotNull(values, "values");

            EntityType = entityType;
            Values = values;
        }

        public static EntityKey GetEntityKey(object entity)
        {
            return new EntityKey(entity.GetType().Name, EntityKeyManager.GetKeyValues(entity));
        }

        public override bool Equals(object obj)
        {
            var other = obj as EntityKey;

            if (other == null || other.EntityType != EntityType)
            {
                return false;
            }

            if (other.Values.Length != Values.Length)
            {
                return false;
            }

            for (var i = 0; i < Values.Length; i++)
            {
                if (!other.Values[i].Equals(Values[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = EntityType.GetHashCode();
                for (var i = 0; i < Values.Length; i++)
                {
                    hash *= 397;
                    hash ^= Values[i].GetHashCode();
                }

                return hash;
            }
        }

        public override string ToString()
        {
            return EntityType + "|" + String.Join("-", Values);
        }
    }
}
