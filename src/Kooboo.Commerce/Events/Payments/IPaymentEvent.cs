using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Payments
{
    [ActivityVisible("Payment Events")]
    public interface IPaymentEvent
    {
        int PaymentId { get; }
    }
}
