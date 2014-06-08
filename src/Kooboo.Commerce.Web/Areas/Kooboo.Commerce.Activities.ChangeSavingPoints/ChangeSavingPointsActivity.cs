using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.ChangeSavingPoints
{
    [Dependency(typeof(IActivity), Key = "ChangeSavingPoints")]
    public class ChangeSavingPointsActivity : IActivity
    {
        public string Name
        {
            get
            {
                return "ChangeSavingPoints";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Increase or reduce customer saving points";
            }
        }

        public bool AllowAsyncExecution
        {
            get
            {
                return false;
            }
        }

        private ICommerceDatabase _database;

        public ChangeSavingPointsActivity(ICommerceDatabase database)
        {
            _database = database;
        }

        public bool CanBindTo(Type eventType)
        {
            if (typeof(IOrderEvent).IsAssignableFrom(eventType))
            {
                return true;
            }

            return false;
        }

        public void Execute(IEvent @event, ActivityContext context)
        {
            var config = context.GetActivityConfig<ChangeSavingPointsActivityConfig>();
            if (config == null)
            {
                return;
            }

            var customer = GetCustomer(@event);
            if (customer == null)
            {
                return;
            }

            if (config.Action == SavingPointAction.Increase)
            {
                customer.SavingPoints += config.Amount;
            }
            else
            {
                customer.SavingPoints -= config.Amount;
            }

            _database.SaveChanges();
        }

        private Customer GetCustomer(IEvent @event)
        {
            var orderEvent = @event as IOrderEvent;
            var order = _database.GetRepository<Order>().Get(orderEvent.OrderId);
            return order == null ? null : order.Customer;
        }

        public ActivityEditor GetEditor(ActivityRule rule, AttachedActivityInfo attachedActivityInfo)
        {
            var path = String.Format("~/Areas/{0}/Views/Config.cshtml", Strings.AreaName);
            return new ActivityEditor(path);
        }
    }
}