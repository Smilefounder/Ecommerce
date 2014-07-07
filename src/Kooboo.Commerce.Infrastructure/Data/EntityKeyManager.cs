using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public static class EntityKeyManager
    {
        static ConcurrentDictionary<Type, List<PropertyInfo>> _cache = new ConcurrentDictionary<Type, List<PropertyInfo>>();

        public static object[] GetKeyValues(object entity)
        {
            Require.NotNull(entity, "entity");

            var props = GetKeyProperties(entity.GetType());
            if (props.Count == 0)
                throw new InvalidOperationException("No keys are defined in type " + entity.GetType() + ".");

            var keys = new object[props.Count];
            for (var i = 0; i < props.Count; i++)
            {
                keys[i] = props[i].GetValue(entity, null);
            }

            return keys;
        }

        public static IList<PropertyInfo> GetKeyProperties(Type entityType)
        {
            Require.NotNull(entityType, "entityType");
            return _cache.GetOrAdd(entityType, LoadKeyProperties).ToList();
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
    }
}
