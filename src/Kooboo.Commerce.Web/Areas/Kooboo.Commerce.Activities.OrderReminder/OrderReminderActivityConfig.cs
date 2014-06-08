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

        public string CancelCondition { get; set; }
    }
}