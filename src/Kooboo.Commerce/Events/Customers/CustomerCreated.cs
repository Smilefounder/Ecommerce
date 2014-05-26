using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Customers
{
    [Serializable]
    [Event(Order = 100)]
    public class CustomerCreated : DomainEvent, ICustomerEvent
    {
        [Reference(typeof(Customer))]
        public int CustomerId { get; set; }

        public CustomerCreated() { }

        public CustomerCreated(Customer customer)
        {
            CustomerId = customer.Id;
        }
    }
}