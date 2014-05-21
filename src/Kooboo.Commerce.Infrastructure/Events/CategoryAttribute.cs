using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class CategoryAttribute : Attribute
    {
        public string Name { get; private set; }

        public CategoryAttribute(string name)
        {
            Name = name;
        }
    }
}
