using Kooboo.Commerce.Orders;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public static class PaymentReturnUrlUtil
    {
        public static string AppendOrderInfoToQueryString(string returnUrl, Order order)
        {
            var parameters = new NameValueCollection();
            parameters["orderId"] = order.Id.ToString();
            parameters["status"] = order.PaymentStatus.ToString();

            var query = new StringBuilder();
            var first = true;

            foreach (var key in parameters.AllKeys)
            {
                if (!first)
                {
                    query.Append("&");
                }
                query.AppendFormat("{0}={1}", key, parameters[key]);

                first = false;
            }

            var queryString = query.ToString();

            if (returnUrl.IndexOf('?') >= 0)
            {
                return returnUrl + "&" + queryString;
            }

            return returnUrl + "?" + queryString;
        }
    }
}
