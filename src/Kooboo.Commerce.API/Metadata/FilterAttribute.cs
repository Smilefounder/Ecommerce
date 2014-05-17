using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Metadata
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class FilterAttribute : Attribute
    {
        public string Name { get; set; }

        public FilterAttribute() { }

        public FilterAttribute(string name)
        {
            Name = name;
        }
    }
}
