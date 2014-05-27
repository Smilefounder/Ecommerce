using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class EventCategory
    {
        public string Name { get; set; }

        public List<EventRules> Events { get; set; }

        public EventCategory()
        {
            Events = new List<EventRules>();
        }
    }

    public class EventRules
    {
        public string EventDisplayName { get; set; }

        public Type EventType { get; set; }

        public List<ActivityRule> Rules { get; set; }

        public EventRules()
        {
            Rules = new List<ActivityRule>();
        }
    }
}