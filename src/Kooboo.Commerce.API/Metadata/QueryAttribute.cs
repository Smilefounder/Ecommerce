using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Metadata
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class QueryAttribute : Attribute
    {
        public string Name { get; set; }

        public QueryAttribute() { }

        public QueryAttribute(string name)
        {
            Name = name;
        }
    }
}
