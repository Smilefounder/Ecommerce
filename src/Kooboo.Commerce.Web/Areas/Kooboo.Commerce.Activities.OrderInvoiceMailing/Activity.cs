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
            return typeof(IOrderEvent).IsAssignableFrom(eventType);
        }

        public ActivityResponse Execute(IEvent evnt, ActivityBinding binding)
        {
            var order = ((IOrderEvent)evnt).Order;

            var settings = JsonConvert.DeserializeObject<ActivityData>(binding.ActivityData);

            var subject = settings.SubjectTemplate;
            var body = settings.BodyTemplate;
            
            // Send mail
            File.WriteAllText("D:\\" + evnt.GetType().Name + ".txt", "[" + DateTime.Now + "] Invoice mail for order #" + order.Id + ", subject: " + subject);

            return ActivityResponse.Continue;
        }
    }
}