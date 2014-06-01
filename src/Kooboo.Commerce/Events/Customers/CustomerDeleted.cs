using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Customers
{
    [Event(Order = 300)]
    public class CustomerDeleted : DomainEvent, ICustomerEvent
    {
        public int CustomerId { get; set; }
    }
}
