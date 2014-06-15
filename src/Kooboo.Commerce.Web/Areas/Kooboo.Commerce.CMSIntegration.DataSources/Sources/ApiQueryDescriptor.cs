using Kooboo.Commerce.API;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    class ApiQueryDescriptor
    {
        public Type ElementType { get; private set; }

        public List<string> IncludablePaths { get; private set; }

        public List<ApiQueryFilterDescriptor> Filters { get; private set; }

        public ApiQueryDescriptor()
        {
            IncludablePaths = new List<string>();
            Filters = new List<ApiQueryFilterDescriptor>();
        }

        static readonly ConcurrentDictionary<Type, ApiQueryDescriptor> _cache = new ConcurrentDictionary<Type, ApiQueryDescriptor>();

        public static ApiQueryDescriptor GetDescriptor(Type queryType)
        {
            if (!queryType.IsInterface)
                throw new ArgumentException("Can only describe query interfaces.");

            return _cache.GetOrAdd(queryType, type => Describe(type));
        }

        static ApiQueryDescriptor Describe(Type type)
        {
            Type elementType = null;
            var contractOpenGeneric = typeof(ICommerceQuery<>);

            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == contractOpenGeneric)
                {
                    elementType = @interface.GetGenericArguments()[0];
                }
            }

            var descriptor = new ApiQueryDescriptor
            {
                ElementType = elementType
            };

            descriptor.Filters = GetQueryFilters(type);
            descriptor.IncludablePaths = GetIncludablePaths(descriptor.ElementType);

            return descriptor;
        }

        static List<ApiQueryFilterDescriptor> GetQueryFilters(Type type)
        {
            var filters = new List<ApiQueryFilterDescriptor>();

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                bool isFilter = false;
                string filterName = null;

                if (method.Name.StartsWith("By") || method.Name.StartsWith("Is") || method.Name.StartsWith("Contains"))
                {
                    isFilter = true;
                }

                if (isFilter)
                {
                    if (String.IsNullOrEmpty(filterName))
                    {
                        filterName = method.Name;
                    }

                    filters.Add(new ApiQueryFilterDescriptor(filterName, method));
                }
            }

            return filters;
        }

        static List<string> GetIncludablePaths(Type elementType)
        {
            var paths = new HashSet<string>();
            var visitedTypes = new HashSet<Type>();

            FindIncludableMembers(elementType, String.Empty, paths, visitedTypes);

            return paths.OrderBy(p => p).ToList();
        }

        static void FindIncludableMembers(Type type, string prefix, HashSet<string> paths, HashSet<Type> visitedTypes)
        {
            if (visitedTypes.Contains(type))
            {
                return;
            }

            visitedTypes.Add(type);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Ignore HAL links
                if (prop.Name == "Links")
                {
                    continue;
                }

                if (!visitedTypes.Contains(prop.PropertyType))
                {
                    Type nextType = null;

                    if (IsOptionalIncludableMember(prop, out nextType))
                    {
                        var path = prefix + prop.Name;
                        paths.Add(path);

                        FindIncludableMembers(nextType, path + ".", paths, visitedTypes);
                    }
                }
            }
        }

        static bool IsOptionalIncludableMember(PropertyInfo property, out Type nextTypeToVisit)
        {
            nextTypeToVisit = null;

            var propType = property.PropertyType;

            if (IsComplexType(propType))
            {
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(propType))
                {
                    Type elementType = null;

                    if (propType.IsArray)
                    {
                        elementType = propType.GetElementType();
                    }
                    else if (property.PropertyType.IsGenericType)
                    {
                        elementType = propType.GetGenericArguments()[0];
                    }

                    if (elementType != null && IsComplexType(elementType))
                    {
                        nextTypeToVisit = elementType;
                        return true;
                    }
                }
                else
                {
                    nextTypeToVisit = propType;
                    return true;
                }
            }

            return false;
        }

        static bool IsComplexType(Type propType)
        {
            return !propType.IsValueType && propType != typeof(String);
        }
    }

    class ApiQueryFilterDescriptor
    {
        public string Name { get; set; }

        public MethodInfo Method { get; set; }

        public List<ApiQueryFilterParameterDescriptor> Parameters { get; set; }

        public ApiQueryFilterDescriptor(string name, MethodInfo method)
        {
            Name = name;
            Method = method;

            var parameters = new List<ApiQueryFilterParameterDescriptor>();

            foreach (var param in method.GetParameters())
            {
                parameters.Add(new ApiQueryFilterParameterDescriptor(param.Name, param.ParameterType));
            }

            Parameters = parameters;
        }

        public SourceFilterDefinition ToFilterDefinition()
        {
            var filter = new SourceFilterDefinition(Name);

            foreach (var param in Parameters)
            {
                filter.Parameters.Add(new FilterParameterDefinition(param.Name, param.Type));
            }

            return filter;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    class ApiQueryFilterParameterDescriptor
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public ApiQueryFilterParameterDescriptor(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}