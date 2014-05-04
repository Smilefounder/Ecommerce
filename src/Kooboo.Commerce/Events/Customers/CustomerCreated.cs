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
    public class CustomerCreated : Event, ICustomerEvent
    {
        [ConditionParameter]
        public int CustomerId { get; set; }

        public CustomerCreated() { }

        public CustomerCreated(Customer customer)
        {
            CustomerId = customer.Id;
        }
    }
}