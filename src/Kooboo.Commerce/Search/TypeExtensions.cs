using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Kooboo.Commerce.Search
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(this Type type, Type attrType)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(o => o.GetCustomAttribute(attrType) != null);
            return props;
        }

        public static T GetCustomAttribute<T>(this Type type)
            where T : Attribute
        {
            var attr = GetCustomAttribute(type, typeof(T));
            if (attr != null)
                return attr as T;
            return null;
        }

        public static object GetCustomAttribute(this Type type, Type attrType)
        {
            var attrs = type.GetCustomAttributes(attrType, true);
            if (attrs != null && attrs.Length > 0)
                return attrs[0];
            return null;
        }

        public static T GetCustomAttribute<T>(this MemberInfo memeberInfo)
            where T : Attribute
        {
            var attr = GetCustomAttribute(memeberInfo, typeof(T));
            if (attr != null)
                return attr as T;
            return null;
        }

        public static object GetCustomAttribute(this MemberInfo memeberInfo, Type attrType)
        {
            var attrs = memeberInfo.GetCustomAttributes(attrType, true);
            if (attrs != null && attrs.Length > 0)
                return attrs[0];
            return null;
        }
    }
}
