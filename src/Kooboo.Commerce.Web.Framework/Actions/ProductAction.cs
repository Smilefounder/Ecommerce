using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Actions
{
    public abstract class ProductAction : IEntityAction
    {
        public abstract string Name { get; }

        public Type EntityType
        {
            get
            {
                return typeof(Product);
            }
        }

        public abstract string ButtonText { get; }

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

        public abstract void Execute(Product product, CommerceInstance instance);

        void IEntityAction.Execute(object entity, Data.CommerceInstance instance)
        {
            Execute(entity as Product, instance);
        }
    }
}
