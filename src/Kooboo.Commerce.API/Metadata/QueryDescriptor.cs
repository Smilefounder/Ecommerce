using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.API.Metadata
{
    /// <summary>
    /// Describes a query api.
    /// </summary>
    public class QueryDescriptor
    {
        /// <summary>
        /// Gets the name of the query.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the query contract, that is, the query api interface type.
        /// </summary>
        public Type QueryType { get; private set; }

        /// <summary>
        /// Gets the type of the elements in the query.
        /// </summary>
        public Type ElementType { get; private set; }

        /// <summary>
        /// Gets the paths of embedded objects that can be optional included.
        /// </summary>
        public IEnumerable<string> OptionalIncludablePaths { get; private set; }

        /// <summary>
        /// Gets the available filters of this query. Filters are special fluent methods.
        /// </summary>
        public ReadOnlyCollection<QueryFilter> Filters { get; private set; }

        private QueryDescriptor()
        {
        }

        public static bool TryDescribe(Type type, out QueryDescriptor descriptor)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            descriptor = null;
            
            if (!type.IsInterface)
            {
                return false;
            }

            var queryAttr = type.GetCustomAttributes(typeof(QueryAttribute), true).OfType<QueryAttribute>().FirstOrDefault();
            if (queryAttr == null)
            {
                return false;
            }

            Type elementType = null;
            var contractOpenGeneric = typeof(ICommerceQuery<>);

            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == contractOpenGeneric)
                {
                    elementType = @interface.GetGenericArguments()[0];
                }
            }

            if (elementType == null)
            {
                return false;
            }

            descriptor = new QueryDescriptor
            {
                QueryType = type,
                Name = String.IsNullOrWhiteSpace(queryAttr.Name) ? Inflector.Pluralize(elementType.Name) : queryAttr.Name,
                ElementType = elementType
            };

            descriptor.Filters = GetQueryFilters(type).AsReadOnly();
            descriptor.OptionalIncludablePaths = GetOptionalIncludablePaths(descriptor.ElementType);

            return true;
        }

        static List<QueryFilter> GetQueryFilters(Type type)
        {
            var filters = new List<QueryFilter>();

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (method.IsDefined(typeof(NonFilterAttribute), true))
                {
                    continue;
                }

                bool isFilter = false;
                string filterName = null;

                var filterAttr = method.GetCustomAttributes(typeof(FilterAttribute), true)
                                       .OfType<FilterAttribute>()
                                       .FirstOrDefault();

                if (filterAttr != null)
                {
                    isFilter = true;
                    filterName = filterAttr.Name;
                }
                else
                {
                    if (method.Name.StartsWith("By") || method.Name.StartsWith("Is") || method.Name.StartsWith("Contains"))
                    {
                        isFilter = true;
                    }
                }

                if (isFilter)
                {
                    if (String.IsNullOrEmpty(filterName))
                    {
                        filterName = method.Name;
                    }

                    filters.Add(new QueryFilter(filterName, method));
                }
            }
            return filters;
        }

        static IEnumerable<string> GetOptionalIncludablePaths(Type elementType)
        {
            var paths = new HashSet<string>();
            var visitedTypes = new HashSet<Type>();

            FindOptionalIncludableMembers(elementType, String.Empty, paths, visitedTypes);

            return paths.OrderBy(p => p).ToList();
        }

        static void FindOptionalIncludableMembers(Type type, string prefix, HashSet<string> paths, HashSet<Type> visitedTypes)
        {
            if (visitedTypes.Contains(type))
            {
                return;
            }

            visitedTypes.Add(type);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!visitedTypes.Contains(prop.PropertyType))
                {
                    Type nextType = null;

                    if (IsOptionalIncludableMember(prop, out nextType))
                    {
                        var path = prefix + prop.Name;
                        paths.Add(path);

                        FindOptionalIncludableMembers(nextType, path + ".", paths, visitedTypes);
                    }
                }
            }
        }

        static bool IsOptionalIncludableMember(PropertyInfo property, out Type nextTypeToVisit)
        {
            nextTypeToVisit = null;

            if (property.IsDefined(typeof(NotSupportOptionalIncludeAttribute), true))
            {
                return false;
            }

            var propType = property.PropertyType;

            if (IsComplexType(propType))
            {
                if (typeof(IEnumerable).IsAssignableFrom(propType))
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
}
