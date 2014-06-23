using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins
{
    static class PluginParametersBuilder
    {
        public static IDictionary<string, object> Build(Type modelType)
        {
            var parameters = new Dictionary<string, object>();
            DoBuild(modelType, null, parameters);
            return parameters;
        }

        static void DoBuild(Type modelType, string prefix, Dictionary<string, object> parameters)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(modelType))
            {
                Type elementType;

                if (IsSimpleType(prop.PropertyType))
                {
                    var propName = prefix + prop.Name;
                    parameters.Add(propName, "{" + propName + "}");
                }
                else if (IsDictionary(prop.PropertyType))
                {
                    // Add dictionary sample
                    var key = prefix + prop.Name + "[Key]";
                    parameters.Add(key, "{" + key + "}");
                }
                else if (IsEnumerable(prop.PropertyType, out elementType))
                {
                    if (IsSimpleType(elementType))
                    {
                        // Add array sample
                        var key = prefix + prop.Name + "[0]";
                        parameters.Add(key, "{" + key + "}");
                    }
                    else
                    {
                        // Add complex array sample
                        DoBuild(elementType, prefix + prop.Name + "[0].", parameters);
                    }
                }
                else
                {
                    DoBuild(prop.PropertyType, prefix + prop.Name + ".", parameters);
                }
            }
        }

        static bool IsSimpleType(Type type)
        {
            return type.IsValueType || type == typeof(String);
        }

        static bool IsEnumerable(Type type, out Type elementType)
        {
            elementType = null;

            if (type.IsArray)
            {
                elementType = type.GetElementType();
                return true;
            }

            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();
                if (genericTypeDef == typeof(IEnumerable<>)
                    || genericTypeDef == typeof(IList<>)
                    || genericTypeDef == typeof(List<>)
                    || genericTypeDef == typeof(ICollection<>))
                {
                    elementType = type.GetGenericArguments()[0];
                    return true;
                }
            }

            return false;
        }

        static bool IsDictionary(Type type)
        {
            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();
                if (genericTypeDef == typeof(IDictionary<,>) || genericTypeDef == typeof(Dictionary<,>))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
