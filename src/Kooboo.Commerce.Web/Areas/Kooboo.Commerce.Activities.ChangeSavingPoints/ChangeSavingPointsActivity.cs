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
    public class ChangeSavingPointsActivity : ActivityBase<IOrderEvent>, IHasCustomActivityParameterEditor
    {
        public override string Name
        {
            get
            {
                return "ChangeSavingPoints";
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Increase or reduce customer saving points";
            }
        }

        public override bool AllowAsyncExecution
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

        protected override void DoExecute(IOrderEvent @event, ActivityContext context)
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

        private Customer GetCustomer(IOrderEvent @event)
        {
            var order = _database.GetRepository<Order>().Get(@event.OrderId);
            return order == null ? null : order.Customer;
        }

        public string GetEditorVirtualPath(ActivityRule rule, AttachedActivityInfo attachedActivityInfo)
        {
            return String.Format("~/Areas/{0}/Views/Config.cshtml", Strings.AreaName);
        }
    }
}