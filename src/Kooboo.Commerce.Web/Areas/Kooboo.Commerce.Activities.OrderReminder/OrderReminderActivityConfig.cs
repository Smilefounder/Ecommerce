using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.OrderReminder
{
    public class OrderReminderActivityConfig
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string Receivers { get; set; }

        public List<Condition> CancelConditions { get; set; }

        public OrderReminderActivityConfig()
        {
            CancelConditions = new List<Condition>();
        }
    }
}