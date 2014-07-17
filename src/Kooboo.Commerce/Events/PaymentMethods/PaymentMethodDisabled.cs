using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.PaymentMethods
{
    [ActivityEvent(Order = 200)]
    public class PaymentMethodDisabled : Event, IPaymentMethodEvent
    {
        [Reference(typeof(PaymentMethod))]
        public int PaymentMethodId { get; set; }

        protected PaymentMethodDisabled() { }

        public PaymentMethodDisabled(PaymentMethod method)
        {
            PaymentMethodId = method.Id;
        }
    }
}
