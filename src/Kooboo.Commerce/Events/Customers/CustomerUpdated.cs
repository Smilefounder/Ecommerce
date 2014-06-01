using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Customers
{
    [Event(Order = 200)]
    public class CustomerUpdated : DomainEvent, ICustomerEvent
    {
        public int CustomerId { get; set; }
    }
}
