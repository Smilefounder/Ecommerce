using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.PaymentMethods
{
    [Event(Order = 100)]
    public class PaymentMethodCreated : DomainEvent, IPaymentMethodEvent
    {
        [Reference(typeof(PaymentMethod))]
        public int PaymentMethodId { get; set; }

        protected PaymentMethodCreated() { }

        public PaymentMethodCreated(PaymentMethod method)
        {
            PaymentMethodId = method.Id;
        }
    }
}
