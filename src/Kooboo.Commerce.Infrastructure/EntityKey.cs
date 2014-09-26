using Kooboo.Commerce.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce
{
    public class EntityKey : IEquatable<EntityKey>
    {
        public Type EntityType { get; private set; }

        // Entity might use composite keys, but that'll complicate things.
        // And it's not often used. So we force to not use composite keys to simplify things.
        public object Value { get; private set; }

        public EntityKey(Type entityType, object value)
        {
            Require.NotNull(entityType, "entityType");
            Require.NotNull(value, "value");

            EntityType = entityType;
            Value = value;
        }

        public bool Equals(EntityKey other)
        {
            if (other == null || other.EntityType != EntityType)
            {
                return false;
            }

            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EntityKey);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EntityType.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }

        public override string ToString()
        {
            return EntityType.Name + ": " + Value;
        }

        #region Factory

        static readonly ConcurrentDictionary<Type, PropertyInfo> _cache = new ConcurrentDictionary<Type, PropertyInfo>();

        public static EntityKey Create<T>(object key)
        {
            return new EntityKey(typeof(T), key);
        }

        public static EntityKey Get(object entity)
        {
            var entityType = entity.GetType();
            // Unwarp entity framework proxy
            entityType = ObjectContext.GetObjectType(entityType);

            return Get(entity, entityType);
        }

        static EntityKey Get(object entity, Type entityType)
        {
            Require.NotNull(entity, "entity");

            var prop = GetKeyProperty(entityType);
            if (prop == null)
                throw new InvalidOperationException("Cannot find id property from entity " + entityType + ".");

            var value = prop.GetValue(entity, null);

            return new EntityKey(entityType, value);
        }

        public static PropertyInfo GetKeyProperty(Type type)
        {
            Require.NotNull(type, "type");
            return _cache.GetOrAdd(type, LoadKeyProperty);
        }

        static PropertyInfo LoadKeyProperty(Type entityType)
        {
            PropertyInfo conventionIdProp = null;

            foreach (var prop in entityType.GetProperties())
            {
                var attr = prop.GetCustomAttribute<KeyAttribute>(false);
                if (attr != null)
                {
                    return prop;
                }

                if (prop.Name == "Id")
                {
                    conventionIdProp = prop;
                }
            }

            // If no explicit keys defined, use the conventional "Id" property as key
            return conventionIdProp;
        }

        #endregion
    }
}
