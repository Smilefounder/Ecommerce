using Kooboo.Commerce.Payments;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web
{
    public class PaymentUrlHelper
    {
        private UrlHelper _urlHelper;

        public PaymentUrlHelper(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string DecorateReturn(string returnUrl, Payment payment)
        {
            var parameters = new NameValueCollection();
            parameters.Add("paymentId", payment.Id.ToString());
            parameters.Add("status", payment.Status.ToString());

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