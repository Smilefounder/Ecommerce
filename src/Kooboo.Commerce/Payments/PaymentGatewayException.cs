using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    [Serializable]
    public class PaymentGatewayException : Exception
    {
        public PaymentGatewayException() { }
        public PaymentGatewayException(string message) : base(message) { }
        public PaymentGatewayException(string message, Exception inner) : base(message, inner) { }
        protected PaymentGatewayException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
