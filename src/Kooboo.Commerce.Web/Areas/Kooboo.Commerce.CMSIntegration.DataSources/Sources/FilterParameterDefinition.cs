using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Sources
{
    public class FilterParameterDefinition
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public FilterParameterDefinition(string name, Type type)
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