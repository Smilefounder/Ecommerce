using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public interface IPaymentProcessor
    {
        string Name { get; }

        IEnumerable<PaymentProcessorParameterDescriptor> ParameterDescriptors { get; }

        ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request);
    }
}
