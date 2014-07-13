using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Actions
{
    public abstract class CustomerAction : IEntityAction
    {
        public abstract string Name { get; }

        public Type EntityType
        {
            get
            {
                return typeof(Customer);
            }
        }

        public virtual string ButtonText
        {
            get
            {
                return Name;
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

        public abstract void Execute(Customer product, CommerceInstance instance);

        void IEntityAction.Execute(object entity, Data.CommerceInstance instance)
        {
            Execute(entity as Customer, instance);
        }
    }
}
