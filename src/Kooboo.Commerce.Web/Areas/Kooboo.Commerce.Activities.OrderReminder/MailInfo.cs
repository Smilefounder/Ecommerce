using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.InvoiceReminder
{
    public class MailInfo
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public List<string> Receivers { get; set; }

        public MailInfo()
        {
            Receivers = new List<string>();
        }
    }
}