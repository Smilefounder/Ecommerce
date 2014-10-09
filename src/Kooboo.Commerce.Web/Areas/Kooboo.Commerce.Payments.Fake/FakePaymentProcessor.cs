using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.Fake
{
    public class FakePaymentProcessor : IPaymentProcessor
    {
        public string Name
        {
            get
            {
                return "Fake";
            }
        }

        public Type ConfigType
        {
            get
            {
                return null;
            }
        }

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public PaymentProcessResult Process(PaymentProcessingContext context)
        {
            var instance = CommerceInstance.Current.Name;
            var redirectUrl = Strings.AreaName
                + "/Home/Gateway?instance=" + instance
                + "&paymentId=" + context.Payment.Id
                + "&currency=" + context.CurrencyCode
                + "&commerceReturnUrl=" + HttpUtility.UrlEncode(context.ReturnUrl);

            redirectUrl = redirectUrl.ToFullUrl(HttpContextAccessor());

            return PaymentProcessResult.Pending(redirectUrl, Guid.NewGuid().ToString("N"));
        }
    }
}