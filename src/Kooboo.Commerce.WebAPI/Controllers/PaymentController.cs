using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class PaymentController : CommerceAPIControllerBase
    {
        public CreatePaymentResult Post(CreatePaymentRequest request)
        {
            return Commerce().Payments.Create(request);
        }
    }
}
