using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Payments
{
    [Event(Category = "Payment Events")]
    public interface IPaymentEvent : IDomainEvent
    {
        int PaymentId { get; }
    }
}
