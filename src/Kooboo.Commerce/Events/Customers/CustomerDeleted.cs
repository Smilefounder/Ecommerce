using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Customers
{
    [Event(Order = 300, ShortName = "Deleted")]
    public class CustomerDeleted : BusinessEvent, ICustomerEvent
    {
        [Param]
        public int CustomerId { get; set; }

        protected CustomerDeleted() { }

        public CustomerDeleted(Customer customer)
        {
            CustomerId = customer.Id;
        }
    }
}
