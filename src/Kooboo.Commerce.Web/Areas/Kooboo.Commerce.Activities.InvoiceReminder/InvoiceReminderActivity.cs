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

namespace Kooboo.Commerce.Activities.InvoiceReminder
{
    [Dependency(typeof(IActivity), Key = "Kooboo.Commerce.Activities.InvoiceReminder.InvoiceReminderActivity")]
    public class InvoiceReminderActivity : IActivity
    {
        public string Name
        {
            get
            {
                return Strings.ActivityName;
            }
        }

        public string DisplayName
        {
            get
            {
                return "Invoice reminder";
            }
        }

        public bool CanBindTo(Type eventType)
        {
            return typeof(IOrderEvent).IsAssignableFrom(eventType);
        }

        public ActivityResult Execute(IEvent evnt, ActivityExecutionContext context)
        {
            var order = ((IOrderEvent)evnt).Order;

            var settings = JsonConvert.DeserializeObject<InvoiceReminderSettings>(context.AttachedActivity.ActivityData);

            var subject = settings.SubjectTemplate;
            var body = settings.BodyTemplate;
            
            // Send mail
            var message =  "[" + DateTime.Now + "] #" + order.Id + ", " + subject + Environment.NewLine;
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\InvoiceReminder.txt");
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, message, Encoding.UTF8);
            } else {
                File.AppendAllText(filePath, message, Encoding.UTF8);
            }

            return ActivityResult.Continue;
        }
    }
}