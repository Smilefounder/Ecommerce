using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Payments
{
    public class RestPaymentAPI : RestPaymentQuery, IPaymentAPI
    {
        public CreatePaymentResult Create(CreatePaymentRequest request)
        {
            return Post<CreatePaymentResult>(null, request);
        }
    }
}
