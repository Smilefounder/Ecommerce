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
    public class InvoiceReminderActivity : IActivity
    {
        public ActivityResult Execute(IEvent evnt, ActivityContext context)
        {
            var brand = (IBrandEvent)evnt;
            //var order = ((IOrderEvent)evnt).Order;

            //var settings = JsonConvert.DeserializeObject<InvoiceReminderSettings>(context.AttachedActivity.ActivityData);

            //var subject = settings.SubjectTemplate;
            //var body = settings.BodyTemplate;
            
            //// Send mail
            //var message =  "[" + DateTime.Now + "] #" + order.Id + ", " + subject + Environment.NewLine;
            //var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\InvoiceReminder.txt");
            var message = "[" + DateTime.Now + "] Event: " + evnt.GetType().Name + ", #" + brand.BrandId + ", Event time: " + evnt.TimestampUtc.ToLocalTime() + Environment.NewLine;
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\InvoiceReminder.txt");
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, message, Encoding.UTF8);
            }
            else
            {
                File.AppendAllText(filePath, message, Encoding.UTF8);
            }

            return ActivityResult.Continue;
        }
    }
}