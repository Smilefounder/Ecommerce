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
using Kooboo.Commerce.Rules;

namespace Kooboo.Commerce.Activities.OrderReminder
{
    public class OrderReminderActivity : ActivityBase<IOrderEvent>, IHasCustomActivityConfigEditor
    {
        public override string Name
        {
            get
            {
                return "OrderReminder";
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Order Reminder";
            }
        }

        public override bool AllowAsyncExecution
        {
            get
            {
                return true;
            }
        }

        public override Type ConfigModelType
        {
            get
            {
                return typeof(OrderReminderActivityConfig);
            }
        }

        private IOrderService _orderService;

        public OrderReminderActivity(IOrderService orderService)
        {
            _orderService = orderService;
        }

        protected override void Execute(IOrderEvent @event, ActivityContext context)
        {
            var config = context.Config as OrderReminderActivityConfig;
            if (config == null || String.IsNullOrWhiteSpace(config.Receivers))
            {
                return;
            }

            var order = _orderService.GetById(@event.OrderId);
            if (order != null)
            {
                if (config.CancelConditions != null && config.CancelConditions.Count > 0)
                {
                    var dataContext = new CancelConditionModel
                    {
                        OrderStatus = order.OrderStatus
                    };

                    if (new RuleEngine().CheckConditions(config.CancelConditions, dataContext))
                    {
                        return;
                    }
                }

                var receivers = config.Receivers.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var mailInfo = new MailInfo
                {
                    Subject = Template.Render(config.Subject, order),
                    Body = Template.Render(config.Body, order)
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

        public string GetEditorVirtualPath(ActivityRule rule, AttachedActivityInfo attachedActivityInfo)
        {
            return "~/Areas/" + Strings.AreaName + "/Views/Config.cshtml";
        }
    }
}