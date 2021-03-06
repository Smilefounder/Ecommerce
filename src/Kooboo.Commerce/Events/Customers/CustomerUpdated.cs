﻿using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Customers
{
    public class CustomerUpdated : Event, ICustomerEvent
    {
        [Reference(typeof(Customer))]
        public int CustomerId { get; set; }

        protected CustomerUpdated() { }

        public CustomerUpdated(Customer customer)
        {
            CustomerId = customer.Id;
        }
    }
}
