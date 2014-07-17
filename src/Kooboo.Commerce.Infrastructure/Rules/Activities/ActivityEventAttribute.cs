using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Activities
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ActivityEventAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public string ShortName { get; set; }

        public string Category { get; set; }

        public int Order { get; set; }

        public ActivityEventAttribute()
        {
            Order = 100;
        }
    }
}
