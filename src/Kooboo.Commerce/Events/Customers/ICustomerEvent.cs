using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Customers
{
    public interface ICustomerEvent : IDomainEvent
    {
        int CustomerId { get; }
    }
}
