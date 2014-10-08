using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.PaymentMethods
{
    public class PaymentMethodEnabled : IPaymentMethodEvent
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
