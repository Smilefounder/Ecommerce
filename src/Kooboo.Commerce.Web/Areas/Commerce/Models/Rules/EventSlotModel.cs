using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class EventSlotModel
    {
        public string EventName { get; set; }

        public IList<RuleModelBase> Rules { get; set; }

        public EventSlotModel()
        {
            Rules = new List<RuleModelBase>();
        }
    }
}