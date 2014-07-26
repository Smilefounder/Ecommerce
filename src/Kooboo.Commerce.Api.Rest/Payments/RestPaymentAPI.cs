using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.RestProvider.Payments
{
    [Dependency(typeof(IPaymentApi), ComponentLifeStyle.Transient)]
    public class RestPaymentAPI : RestPaymentQuery, IPaymentApi
    {
        public PaymentResult Pay(PaymentRequest request)
        {
            return Post<PaymentResult>(null, request);
        }
    }
}
