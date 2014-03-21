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
        public Payment[] Get([FromUri]PaymentQueryParams parameters)
        {
            return Commerce().Payments.By(parameters).ToArray();
        }

        public Payment[] Get([FromUri]PaymentQueryParams parameters, int pageIndex, int pageSize)
        {
            return Commerce().Payments.By(parameters).Pagination(pageIndex, pageSize);
        }

        public Payment GetFirst([FromUri]PaymentQueryParams parameters)
        {
            return Commerce().Payments.By(parameters).FirstOrDefault();
        }

        public int GetCount([FromUri]PaymentQueryParams parameters)
        {
            return Commerce().Payments.By(parameters).Count();
        }

        public CreatePaymentResult Post(CreatePaymentRequest request)
        {
            return Commerce().Payments.Create(request);
        }
    }
}
