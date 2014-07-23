using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Topbar
{
    public abstract class CustomerTopbarCommand : ITopbarCommand
    {
        public abstract string Name { get; }

        public virtual string ButtonText
        {
            get
            {
                return Name;
            }
        }

        public virtual string IconClass
        {
            get { return null; }
        }

        public virtual string ConfirmMessage
        {
            get { return null; }
        }

        public virtual int Order
        {
            get { return 100; }
        }

        public virtual Type ConfigType
        {
            get { return null; }
        }

        public virtual IEnumerable<MvcRoute> ApplyTo
        {
            get
            {
                yield return MvcRoutes.Customers.All();
            }
        }

        public abstract bool CanExecute(Customer customer, CommerceInstance instance);

        bool ITopbarCommand.CanExecute(object dataItem, CommerceInstance instance)
        {
            return CanExecute(dataItem as Customer, instance);
        }

        public abstract ActionResult Execute(IEnumerable<Customer> customers, object config, CommerceInstance instance);

        ActionResult ITopbarCommand.Execute(IEnumerable<object> dataItems, object config, CommerceInstance instance)
        {
            return Execute(dataItems.OfType<Customer>(), config, instance);
        }
    }
}
