using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.PaymentMethods
{
    [Event(Order = 300, ShortName = "Enabled")]
    public class PaymentMethodEnabled : BusinessEvent, IPaymentMethodEvent
    {
        [Reference(typeof(PaymentMethod))]
        public int PaymentMethodId { get; set; }

        protected PaymentMethodEnabled() { }

        public PaymentMethodEnabled(PaymentMethod method)
        {
            PaymentMethodId = method.Id;
        }
    }
}
