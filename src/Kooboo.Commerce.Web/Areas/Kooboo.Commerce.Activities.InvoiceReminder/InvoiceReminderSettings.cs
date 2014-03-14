using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.InvoiceReminder
{
    public class InvoiceReminderSettings
    {
        [Display(Name = "Subject template")]
        public string SubjectTemplate { get; set; }

        [Display(Name = "Body template")]
        public string BodyTemplate { get; set; }
    }
}