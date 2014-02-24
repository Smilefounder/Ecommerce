using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class EventCategoryAttribute : Attribute
    {
        public string Name { get; set; }

        public EventCategoryAttribute(string name)
        {
            Name = name;
        }
    }
}
