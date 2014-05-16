using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.API.Metadata
{
    public class QueryDescriptor
    {
        public string Name { get; private set; }

        public Type QueryType { get; private set; }

        public Type ElementType { get; private set; }

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
                    if (method.Name.StartsWith("By") || method.Name.StartsWith("Is"))
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

            descriptor.Filters = filters.AsReadOnly();

            return true;
        }
    }
}
