using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Actions.Events
{
    public class GetCustomerActions : Event
    {
        [Reference(typeof(Customer))]
        public int CustomerId { get; private set; }

        public IList<string> ActionNames { get; private set; }

        public GetCustomerActions(int customerId)
        {
            CustomerId = customerId;
            ActionNames = new List<string>();
        }
    }
}
