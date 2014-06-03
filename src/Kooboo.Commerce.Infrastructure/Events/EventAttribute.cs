using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EventAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public int Order { get; set; }

        public EventAttribute()
        {
            Order = 100;
        }

        public EventAttribute(string displayName) : this()
        {
            DisplayName = displayName;
        }
    }
}
