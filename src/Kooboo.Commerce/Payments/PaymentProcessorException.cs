using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    [Serializable]
    public class PaymentProcessorException : Exception
    {
        public PaymentProcessorException() { }
        public PaymentProcessorException(string message) : base(message) { }
        public PaymentProcessorException(string message, Exception inner) : base(message, inner) { }
        protected PaymentProcessorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
