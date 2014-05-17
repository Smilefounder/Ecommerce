using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.API.Metadata
{
    /// <summary>
    /// Represents the metadata of a filter method in a query api.
    /// </summary>
    public class QueryFilter
    {
        public string Name { get; private set; }

        public MethodInfo Method { get; private set; }

        public ReadOnlyCollection<QueryFilterParameter> Parameters { get; private set; }

        public QueryFilter(string name, MethodInfo method)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("'name' is required.", "name");

            if (method == null)
                throw new ArgumentNullException("method");

            Name = name;
            Method = method;

            var parameters = new List<QueryFilterParameter>();

            foreach (var param in method.GetParameters())
            {
                parameters.Add(new QueryFilterParameter(param.Name, param.ParameterType));
            }

            Parameters = parameters.AsReadOnly();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class QueryFilterParameter
    {
        public string Name { get; private set; }

        public Type Type { get; private set; }

        public QueryFilterParameter(string name, Type type)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("'name' is required.", "name");

            if (type == null)
                throw new ArgumentNullException("type");

            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
