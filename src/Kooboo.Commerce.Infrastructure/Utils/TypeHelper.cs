using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Utils
{
    public static class TypeHelper
    {
        public static PropertyInfo GetProperty(Type type, string property)
        {
            Require.NotNull(type, "type");
            Require.NotNullOrEmpty(property, "property");

            var parts = property.Split('.');
            Type containerType = type;
            PropertyInfo result = null;

            foreach (var part in parts)
            {
                result = containerType.GetProperty(part, BindingFlags.Public | BindingFlags.Instance);
                if (result != null)
                {
                    containerType = result.PropertyType;
                }
                else
                {
                    break;
                }
            }

            return result;
        }
    }
}
