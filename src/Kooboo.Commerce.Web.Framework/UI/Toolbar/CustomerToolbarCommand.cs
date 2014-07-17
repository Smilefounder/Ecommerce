using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Toolbar
{
    public abstract class CustomerToolbarCommand : IToolbarCommand
    {
        public abstract string Name { get; }

        public abstract string ButtonText { get; }

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

        bool IToolbarCommand.CanExecute(object data, CommerceInstance instance)
        {
            return CanExecute(data as Customer, instance);
        }

        public abstract ToolbarCommandResult Execute(Customer customer, object config, CommerceInstance instance);

        ToolbarCommandResult IToolbarCommand.Execute(object data, object config, CommerceInstance instance)
        {
            return Execute(data as Customer, config, instance);
        }
    }
}
