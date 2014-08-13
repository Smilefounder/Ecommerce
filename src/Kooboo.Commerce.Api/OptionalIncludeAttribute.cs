using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.Api
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OptionalIncludeAttribute : Attribute
    {
        static readonly ConcurrentDictionary<Type, HashSet<string>> _cache = new ConcurrentDictionary<Type, HashSet<string>>();

        public static IEnumerable<string> GetOptionalIncludeFields(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return _cache.GetOrAdd(type, key =>
            {
                var fields = new HashSet<string>();
                FindOptionalIncludeFields(key, null, fields, new HashSet<PropertyInfo>());
                return fields;
            });
        }

        static void FindOptionalIncludeFields(Type type, string prefix, HashSet<string> paths, HashSet<PropertyInfo> visitedProperties)
        {
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (visitedProperties.Contains(prop))
                {
                    continue;
                }

                var attr = prop.GetCustomAttributes(typeof(OptionalIncludeAttribute), true).OfType<OptionalIncludeAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    paths.Add(prefix + prop.Name);
                    visitedProperties.Add(prop);

                    if (IsComplexType(prop.PropertyType))
                    {
                        Type nextType = null;

                        if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType))
                        {
                            if (prop.PropertyType.IsArray)
                            {
                                nextType = prop.PropertyType.GetElementType();
                            }
                            else if (prop.PropertyType.IsGenericType)
                            {
                                // Ignore dictionary (we don't want to support such complex senario)
                                if (!prop.PropertyType.GetInterfaces().Any(it => it.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                                {
                                    nextType = prop.PropertyType.GetGenericArguments()[0];
                                }
                            }
                        }
                        else
                        {
                            nextType = prop.PropertyType;
                        }

                        if (nextType != null)
                        {
                            FindOptionalIncludeFields(nextType, prefix + prop.Name + ".", paths, visitedProperties);
                        }
                    }
                }
            }
        }

        static bool IsComplexType(Type type)
        {
            return !type.IsValueType && type != typeof(String);
        }
    }
}
