using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.Web.Framework.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
{
    public class PaymentController : CommerceAPIControllerQueryBase<Payment>
    {
        [Transactional]
        public PaymentResult Post(PaymentRequest request)
        {
            return Commerce().Payments.Pay(request);
        }

        protected override ICommerceQuery<Payment> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().Payments.Query();

            if (Request.GetRouteData().Values.Keys.Contains("id"))
                query = query.ById(Convert.ToInt32(Request.GetRouteData().Values["id"]));
            if (!string.IsNullOrEmpty(qs["id"]))
            {
                query = query.ById(Convert.ToInt32(qs["id"]));
            }
            if (!string.IsNullOrEmpty(qs["targetType"]) && !string.IsNullOrEmpty(qs["targetId"]))
            {
                query = query.ByTarget(qs["targetType"], qs["targetId"]);
            }
            if (!string.IsNullOrEmpty(qs["status"]))
            {
                var status = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), qs["status"]);
                query = query.ByStatus(status);
            }

            return BuildLoadWithFromQueryStrings(query, qs);
        }
    }
}
