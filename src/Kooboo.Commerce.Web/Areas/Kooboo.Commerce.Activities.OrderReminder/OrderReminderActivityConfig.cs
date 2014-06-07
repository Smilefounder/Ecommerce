using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.InvoiceReminder
{
    public class OrderReminderActivityConfig
    {
        public string SubjectTemplate { get; set; }

        public string BodyTemplate { get; set; }

        public string Receivers { get; set; }
    }
}