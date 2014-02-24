using Kooboo.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Customers
{
    public class CustomerCreated : ICustomerEvent
    {
        public Customer Customer { get; private set; }

        public CustomerCreated(Customer customer)
        {
            Customer = customer;
        }
    }
}