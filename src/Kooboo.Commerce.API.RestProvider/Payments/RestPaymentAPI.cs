using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.Payments
{
    [Dependency(typeof(IPaymentAPI))]
    public class RestPaymentAPI : RestPaymentQuery, IPaymentAccess, IPaymentAPI
    {
        public CreatePaymentResult Create(CreatePaymentRequest request)
        {
            return Post<CreatePaymentResult>(null, request);
        }

        public IPaymentQuery Query()
        {
            return this;
        }

        public IPaymentAccess Access()
        {
            return this;
        }
    }
}
