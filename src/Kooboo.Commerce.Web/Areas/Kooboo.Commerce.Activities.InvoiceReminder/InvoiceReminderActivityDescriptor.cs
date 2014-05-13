using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Events.Brands;
using Kooboo.Commerce.Events.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.InvoiceReminder
{
    [Dependency(typeof(IActivityDescriptor), ComponentLifeStyle.Singleton, Key = "InvoiceReminderActivityDescriptor")]
    public class InvoiceReminderActivityDescriptor : IActivityDescriptor
    {
        public string Name
        {
            get
            {
                return "Invoice Reminder";
            }
        }

        public Type ActivityType
        {
            get
            {
                return typeof(InvoiceReminderActivity);
            }
        }

        public bool AllowAsyncExecution
        {
            get
            {
                return true;
            }
        }

        public bool Configurable
        {
            get
            {
                return true;
            }
        }

        public string ConfigViewVirtualPath
        {
            get
            {
                return "~/Areas/" + Strings.AreaName + "/Views/Config.cshtml";
            }
        }

        public bool CanBindTo(Type eventType)
        {
            return typeof(IBrandEvent).IsAssignableFrom(eventType);
        }
    }
}