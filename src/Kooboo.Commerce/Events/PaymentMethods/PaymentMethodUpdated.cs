using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.PaymentMethods
{
    [Event(Order = 200)]
    public class PaymentMethodUpdated : BusinessEvent, IPaymentMethodEvent
    {
        [Reference(typeof(PaymentMethod))]
        public int PaymentMethodId { get; set; }

        protected PaymentMethodUpdated() { }

        public PaymentMethodUpdated(PaymentMethod method)
        {
            PaymentMethodId = method.Id;
        }
    }
}
