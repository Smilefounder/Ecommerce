using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Payments
{
    [Dependency(typeof(IPaymentAPI), ComponentLifeStyle.Transient)]
    public class RestPaymentAPI : RestPaymentQuery, IPaymentAPI
    {
        public PaymentResult Pay(PaymentRequest request)
        {
            return Post<PaymentResult>(null, request);
        }
    }
}
