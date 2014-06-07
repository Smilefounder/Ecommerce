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
using Kooboo.Commerce.Orders.Services;

namespace Kooboo.Commerce.Activities.InvoiceReminder
{
    [Dependency(typeof(IActivity), Key = "Order Reminder")]
    public class OrderReminderActivity : IActivity
    {
        public string Name
        {
            get
            {
                return "Order Reminder";
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

        private IOrderService _orderService;

        public OrderReminderActivity(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public void Execute(IEvent evnt, ActivityContext context)
        {
            var config = context.GetActivityConfig<OrderReminderActivityConfig>();
            if (config == null || String.IsNullOrWhiteSpace(config.Receivers))
            {
                return;
            }

            var orderEvent = (IOrderEvent)evnt;
            var order = _orderService.GetById(orderEvent.OrderId);
            if (order != null)
            {
                var receivers = config.Receivers.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var mailInfo = new MailInfo
                {
                    Subject = Template.Render(config.SubjectTemplate, order),
                    Body = Template.Render(config.BodyTemplate, order)
                };

                foreach (var receiver in receivers)
                {
                    var email = receiver;
                    if (email.Equals("{Customer}", StringComparison.OrdinalIgnoreCase))
                    {
                        email = order.Customer.Email;
                    }

                    if (!String.IsNullOrWhiteSpace(email))
                    {
                        mailInfo.Receivers.Add(email.Trim());
                    }
                }

                MailClient.Send(mailInfo);
            }
        }

        public ActivityEditor GetEditor(ActivityRule rule, AttachedActivityInfo attachedActivityInfo)
        {
            return new ActivityEditor("~/Areas/" + Strings.AreaName + "/Views/Config.cshtml");
        }
    }
}