using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Customers
{
    [Category("Customers", Order = 400)]
    public interface ICustomerEvent : IBusinessEvent
    {
        int CustomerId { get; }
    }
}
