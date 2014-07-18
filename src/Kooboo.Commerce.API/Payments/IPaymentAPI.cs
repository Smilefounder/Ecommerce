using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public interface IPaymentAPI : IPaymentQuery
    {
        PaymentResult Pay(PaymentRequest request);
    }
}
