using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class ActivityVisibleAttribute : Attribute
    {
        public string EventCategory { get; set; }

        public ActivityVisibleAttribute(string eventCategory)
        {
            EventCategory = eventCategory;
        }
    }
}
