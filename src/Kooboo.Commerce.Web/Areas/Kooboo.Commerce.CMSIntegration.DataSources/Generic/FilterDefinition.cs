using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic
{
    public class FilterDefinition
    {
        public string Name { get; set; }

        public List<FilterParameterDefinition> Parameters { get; set; }

        public FilterDefinition(string name)
        {
            Name = name;
            Parameters = new List<FilterParameterDefinition>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}