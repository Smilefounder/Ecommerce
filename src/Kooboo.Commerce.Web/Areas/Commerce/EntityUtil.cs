using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce
{
    public static class EntityUtil
    {
        public static object GetKey(this object entity)
        {
            object key;

            if (!TryGetKey(entity, out key))
                throw new InvalidOperationException("Cannot resolve key property.");

            return key;
        }

        public static bool TryGetKey(this object entity, out object key)
        {
            Require.NotNull(entity, "entity");

            key = null;

            var prop = GetKeyProperty(entity.GetType());
            if (prop != null)
            {
                key = prop.GetValue(entity, null);
                return true;
            }

            return false;
        }

        static PropertyInfo GetKeyProperty(Type type)
        {
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.IsDefined(typeof(KeyAttribute), true))
                {
                    return prop;
                }
            }

            return type.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        }
    }
}