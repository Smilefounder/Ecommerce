using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Metadata
{
    public class FilterDescription
    {
        public string Name { get; private set; }

        public IEnumerable<ParameterDescription> Parameters { get; private set; }

        public FilterDescription(string name, params ParameterDescription[] parameters)
            : this(name, parameters as IEnumerable<ParameterDescription>)
        {
        }

        public FilterDescription(string name, IEnumerable<ParameterDescription> parameters)
        {
            Name = name;
            Parameters = parameters.ToList();
        }

        public QueryFilter CreateFilter(object parameters)
        {
            return new QueryFilter(Name, parameters);
        }

        public QueryFilter CreateFilter(IDictionary<string, object> parameters)
        {
            return new QueryFilter(Name, parameters);
        }
    }
}
