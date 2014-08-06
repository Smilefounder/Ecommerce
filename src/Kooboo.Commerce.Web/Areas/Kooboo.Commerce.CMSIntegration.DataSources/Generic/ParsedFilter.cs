using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic
{
    public class ParsedFilter
    {
        public string Name { get; set; }

        public IDictionary<string, object> ParameterValues { get; set; }

        public ParsedFilter(string name)
        {
            Name = name;
            ParameterValues = new Dictionary<string, object>();
        }

        public object GetParameterValue(string paramName)
        {
            object value;
            if (ParameterValues.TryGetValue(paramName, out value))
            {
                return value;
            }

            return null;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}