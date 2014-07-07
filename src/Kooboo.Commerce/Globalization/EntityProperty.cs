using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Globalization
{
    public class EntityProperty
    {
        public EntityKey EntityKey { get; private set; }

        public string Name { get; private set; }

        public EntityProperty(string name, EntityKey entityKey)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(entityKey, "entityKey");

            Name = name;
            EntityKey = entityKey;
        }

        public override string ToString()
        {
            return EntityKey + "|" + Name;
        }

        public override bool Equals(object obj)
        {
            var other = obj as EntityProperty;
            return other != null
                && other.EntityKey.Equals(other.EntityKey)
                && other.Name == Name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EntityKey.GetHashCode() * 397) ^ Name.GetHashCode();
            }
        }
    }
}
