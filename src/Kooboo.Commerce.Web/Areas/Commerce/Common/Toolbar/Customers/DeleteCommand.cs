using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Web.Framework.UI.Toolbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Toolbar.Customers
{
    public class DeleteCommand : CustomerToolbarCommand
    {
        public override string Name
        {
            get
            {
                return "Delete";
            }
        }

        public override string IconClass
        {
            get
            {
                return "delete";
            }
        }

        public override string ButtonText
        {
            get
            {
                return "Delete";
            }
        }

        public override string ConfirmMessage
        {
            get
            {
                return "Are you sure to delete this customer?";
            }
        }

        public override bool CanExecute(Kooboo.Commerce.Customers.Customer customer, Data.CommerceInstance instance)
        {
            return true;
        }

        public override ToolbarCommandResult Execute(Kooboo.Commerce.Customers.Customer customer, object config, Data.CommerceInstance instance)
        {
            var service = EngineContext.Current.Resolve<ICustomerService>();
            service.Delete(customer);
            return null;
        }
    }
}