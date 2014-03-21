using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class PaymentMethodController : CommerceAPIControllerBase
    {
        public PaymentMethod[] Get(PaymentMethodType? type)
        {
            var query = Commerce().PaymentMethods;

            if (type != null)
            {
                query.ByType(type.Value);
            }

            return query.ToArray();
        }
    }
}
