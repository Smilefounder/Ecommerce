using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Common.Runtime.Dependency;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Events.Customers;
using System.Text;
using Kooboo.Commerce.Events.Brands;

namespace Kooboo.Commerce.Activities.InvoiceReminder
{
    [Dependency(typeof(IActivity), Key = "Invoice Reminder")]
    public class InvoiceReminderActivity : IActivity
    {
        public string Name
        {
            get
            {
                return "Invoice Reminder";
            }
        }

        public bool AllowAsyncExecution
        {
            get
            {
                return true;
            }
        }

        public bool CanBindTo(Type eventType)
        {
            return typeof(IOrderEvent).IsAssignableFrom(eventType);
        }

        public void Execute(IEvent evnt, ActivityContext context)
        {
        }

        public ActivityEditor GetEditor(ActivityRule rule, AttachedActivityInfo attachedActivityInfo)
        {
            return new ActivityEditor("~/Areas/" + Strings.AreaName + "/Views/Config.cshtml");
        }
    }
}