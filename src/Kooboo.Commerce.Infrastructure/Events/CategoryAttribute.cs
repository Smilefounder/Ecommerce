using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class CategoryAttribute : Attribute
    {
        /// <summary>
        /// Category name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Category display order.
        /// </summary>
        public int Order { get; set; }

        public CategoryAttribute(string name)
        {
            Name = name;
            Order = 100;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
