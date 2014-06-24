using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    public static class DataSourceDefinitionHelper
    {
        public static IDictionary<string, object> GetDefinitions(Type modelType)
        {
            var definitions = new Dictionary<string, object>();
            FindDefinitions(modelType, null, definitions, new HashSet<Type>());

            return definitions;
        }

        static void FindDefinitions(Type modelType, string prefix, IDictionary<string, object> definitions, HashSet<Type> visitedTypes)
        {
            foreach (var prop in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (IsSimpleType(prop.PropertyType))
                {
                    definitions.Add(prefix + prop.Name, null);
                }
                else if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)) // Ignore collections
                {
                    if (!visitedTypes.Contains(prop.PropertyType))
                    {
                        visitedTypes.Add(prop.PropertyType);
                        FindDefinitions(prop.PropertyType, prefix + prop.Name + ".", definitions, visitedTypes);
                    }
                }
            }
        }

        static bool IsSimpleType(Type type)
        {
            return type.IsValueType || type == typeof(String);
        }
    }
}