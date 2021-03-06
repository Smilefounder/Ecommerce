﻿using Kooboo.CMS.Common.Runtime.Dependency;
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

namespace Kooboo.Commerce.Rules.Activities.ChangeSavingPoints
{
    public class ChangeSavingPointsActivity : Activity<IOrderEvent>, IHasCustomActivityConfigEditor
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

        public override Type ConfigType
        {
            get
            {
                return typeof(ChangeSavingPointsActivityConfig);
            }
        }

        private ICommerceDatabase _database;

        public ChangeSavingPointsActivity(ICommerceDatabase database)
        {
            _database = database;
        }

        protected override void Execute(IOrderEvent @event, ActivityContext context)
        {
            var config = context.Config as ChangeSavingPointsActivityConfig;
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
            var order = _database.Repository<Order>().Find(@event.OrderId);
            return order == null ? null : order.Customer;
        }

        public string GetEditorVirtualPath()
        {
            return "~/Areas/Kooboo.Commerce.Rules.Activities.ChangeSavingPoints/Views/Config.cshtml";
        }
    }
}