using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Toolbar
{
    public abstract class ProductToolbarCommand : IToolbarCommand
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

        public virtual Type ConfigType
        {
            get
            {
                return null;
            }
        }

        public virtual IEnumerable<MvcRoute> ApplyTo
        {
            get
            {
                yield return MvcRoutes.Products.All();
            }
        }

        public abstract bool IsVisible(Product product, CommerceInstance instance);

        bool IToolbarCommand.CanExecute(object context, CommerceInstance instance)
        {
            return IsVisible(context as Product, instance);
        }

        public abstract ToolbarCommandResult Execute(Product product, object config, CommerceInstance instance);

        ToolbarCommandResult IToolbarCommand.Execute(object data, object config, CommerceInstance instance)
        {
            return Execute(data as Product, config, instance);
        }
    }
}
