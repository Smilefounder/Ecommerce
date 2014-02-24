using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce
{
    public static class TypeExtensions
    {
        public static string GetDescription(this Type type)
        {
            var attr = type.GetCustomAttributes(typeof(DescriptionAttribute), false)
                           .OfType<DescriptionAttribute>()
                           .FirstOrDefault();

            return attr == null ? null : attr.Description;
        }

        public static string GetVersionUnawareAssemblyQualifiedName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        public static PropertyInfo GetKeyProperty(this Type type)
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
