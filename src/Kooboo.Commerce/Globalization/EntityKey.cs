using Kooboo.Commerce.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Globalization
{
    public class EntityKey : IEquatable<EntityKey>
    {
        public Type EntityType { get; private set; }

        public object[] Values { get; private set; }

        public EntityKey(Type entityType, object[] values)
        {
            Require.NotNull(entityType, "entityType");
            Require.NotNull(values, "values");

            EntityType = entityType;
            Values = values;
        }

        public bool Equals(EntityKey other)
        {
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

        public override bool Equals(object obj)
        {
            return Equals(obj as EntityKey);
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
            return EntityType.Name + "|" + String.Join("-", Values);
        }

        #region Factory

        static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _cache = new ConcurrentDictionary<Type, List<PropertyInfo>>();

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

            var props = GetKeyProperties(entityType);
            if (props.Count == 0)
                throw new InvalidOperationException("No key is defined in type " + entity.GetType() + ".");

            var idValues = new object[props.Count];
            for (var i = 0; i < props.Count; i++)
            {
                idValues[i] = props[i].GetValue(entity, null);
            }

            return new EntityKey(entityType, idValues);
        }

        public static IList<PropertyInfo> GetKeyProperties(Type type)
        {
            Require.NotNull(type, "type");
            return _cache.GetOrAdd(type, LoadKeyProperties).ToList();
        }

        static List<PropertyInfo> LoadKeyProperties(Type entityType)
        {
            var props = new List<PropertyInfo>();
            PropertyInfo conventionIdProp = null;

            foreach (var prop in entityType.GetProperties())
            {
                var attr = prop.GetCustomAttribute<KeyAttribute>(false);
                if (attr != null)
                {
                    props.Add(prop);
                }

                if (prop.Name == "Id")
                {
                    conventionIdProp = prop;
                }
            }

            // If no explicit keys defined, use the conventional "Id" property as key
            if (props.Count == 0)
            {
                props.Add(conventionIdProp);
            }

            return props;
        }

        #endregion
    }
}
