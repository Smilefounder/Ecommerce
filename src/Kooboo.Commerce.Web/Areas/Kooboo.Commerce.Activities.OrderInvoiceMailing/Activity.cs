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

namespace Kooboo.Commerce.Activities.OrderInvoiceMailing
{
    [Dependency(typeof(IActivity), Key = "Kooboo.Commerce.Activities.OrderInvoiceMailing.Activity")]
    public class Activity : IActivity
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
            return typeof(ICustomerEvent).IsAssignableFrom(eventType);
        }

        public ActivityResult Execute(IEvent evnt, ActivityExecutionContext context)
        {
            var order = ((IOrderEvent)evnt).Order;

            var settings = JsonConvert.DeserializeObject<ActivityData>(context.AttachedActivity.ActivityData);

            var subject = settings.SubjectTemplate;
            var body = settings.BodyTemplate;
            
            // Send mail
            File.WriteAllText("D:\\" + evnt.GetType().Name + ".txt", "[" + DateTime.Now + "] Invoice mail for order #" + order.Id + ", subject: " + subject);

            return ActivityResult.Continue;
        }
    }
}