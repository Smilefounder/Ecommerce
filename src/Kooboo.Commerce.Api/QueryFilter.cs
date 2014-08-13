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
            Parameters = ObjectHelper.AnonymousToDictionary(parameters);
        }

        public QueryFilter(string name, IDictionary<string, object> parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }
}
