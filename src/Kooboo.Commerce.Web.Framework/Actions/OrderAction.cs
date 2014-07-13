using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Actions
{
    public abstract class OrderAction : IEntityAction
    {
        public abstract string Name { get; }

        public virtual string ButtonText
        {
            get
            {
                return Name;
            }
        }

        public Type EntityType
        {
            get
            {
                return typeof(Order);
            }
        }

        public virtual string IconClass
        {
            get
            {
                return null;
            }
        }

        public virtual string ConfirmMessage
        {
            get
            {
                return null;
            }
        }

        public virtual int Order
        {
            get
            {
                return 100;
            }
        }

        public abstract void Execute(Order order, CommerceInstance instance);

        void IEntityAction.Execute(object entity, CommerceInstance instance)
        {
            Execute(entity as Order, instance);
        }
    }
}
