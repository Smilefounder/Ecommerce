using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Toolbar
{
    public abstract class OrderToolbarCommand : IToolbarCommand
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
                yield return MvcRoutes.Orders.All();
            }
        }

        public abstract bool IsVisible(Order order, CommerceInstance instance);

        bool IToolbarCommand.CanExecute(object context, CommerceInstance instance)
        {
            return IsVisible(context as Order, instance);
        }

        public abstract ToolbarCommandResult Execute(Order order, object config, CommerceInstance instance);

        ToolbarCommandResult IToolbarCommand.Execute(object data, object config, CommerceInstance instance)
        {
            return Execute(data as Order, config, instance);
        }
    }
}
