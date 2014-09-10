using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Customers
{
    public class CustomerDeleted : Event, ICustomerEvent
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
