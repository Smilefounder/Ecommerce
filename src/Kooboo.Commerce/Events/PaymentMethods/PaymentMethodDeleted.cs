using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.PaymentMethods
{
    [Event(Order = 500)]
    public class PaymentMethodDeleted : DomainEvent, IPaymentMethodEvent
    {
        [Param]
        public int PaymentMethodId { get; set; }

        [Param]
        public string PaymentMethodName { get; set; }

        protected PaymentMethodDeleted() { }

        public PaymentMethodDeleted(PaymentMethod method)
        {
            PaymentMethodId = method.Id;
            PaymentMethodName = method.Name;
        }
    }
}
