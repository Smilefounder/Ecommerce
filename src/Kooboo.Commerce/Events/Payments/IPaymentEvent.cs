using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Payments
{
    [Category("Payments", Order = 1100)]
    public interface IPaymentEvent : IDomainEvent
    {
        int PaymentId { get; }
    }
}
