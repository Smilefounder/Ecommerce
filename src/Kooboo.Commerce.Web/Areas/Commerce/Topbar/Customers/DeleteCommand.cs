using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Topbar.Customers
{
    public class DeleteCommand : CustomerTopbarCommand
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
                return "Are you sure to delete selected customers?";
            }
        }

        public override bool CanExecute(Kooboo.Commerce.Customers.Customer customer, CommerceInstance instance)
        {
            return true;
        }

        public override ActionResult Execute(IEnumerable<Customer> customers, object config, CommerceInstance instance)
        {
            var service = new CustomerService(instance);
            foreach (var customer in customers)
            {
                service.Delete(customer);
            }

            return null;
        }
    }
}