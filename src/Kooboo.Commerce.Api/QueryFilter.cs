using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public class QueryFilter
    {
        public string Name { get; set; }

        public IDictionary<string, object> Parameters { get; set; }

        public QueryFilter()
        {
            Parameters = new Dictionary<string, object>();
        }

        public QueryFilter(string name, object parameters)
        {
            Name = name;
            Parameters = ObjectHelper.AnonymousToDictionary(parameters, StringComparer.OrdinalIgnoreCase);
        }

        public QueryFilter(string name, IDictionary<string, object> parameters)
        {
            Name = name;
            Parameters = new Dictionary<string, object>(parameters, StringComparer.OrdinalIgnoreCase);
        }

        public T GetParameterValueOrDefault<T>(string paramName)
        {
            return GetParameterValueOrDefault<T>(paramName, default(T));
        }

        public T GetParameterValueOrDefault<T>(string paramName, T defaultValue)
        {
            if (Parameters.ContainsKey(paramName))
            {
                var value = Parameters[paramName];
                if (value != null)
                {
                    return (T)value;
                }
            }

            return defaultValue;
        }
    }
}
