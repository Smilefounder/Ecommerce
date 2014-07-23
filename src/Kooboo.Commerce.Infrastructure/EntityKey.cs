using Kooboo.Commerce.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public static EntityKey Get<T>(T entity)
            where T : class
        {
            return Get(entity, typeof(T));
        }

        public static EntityKey Get(object entity)
        {
            return Get(entity, entity.GetType());
        }

        static EntityKey Get(object entity, Type entityType)
        {
            Require.NotNull(entity, "entity");

            var prop = GetKeyProperty(entityType);
            if (prop == null)
                throw new InvalidOperationException("Cannot find id proeprty from entity " + entityType + ".");

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
