using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public interface IPaymentProcessor
    {
        string Name { get; }

        ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request);

        IEnumerable<PaymentType> SupportedPaymentTypes { get; }

        bool SupportMultiplePaymentMethods { get; }

        IEnumerable<SupportedPaymentMethod> GetSupportedPaymentMethods(PaymentType paymentType);
    }
}
