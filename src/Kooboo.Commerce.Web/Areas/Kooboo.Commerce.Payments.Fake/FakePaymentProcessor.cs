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
    [Dependency(typeof(IPaymentProcessor), Key = "Fake")]
    public class FakePaymentProcessor : IPaymentProcessor
    {
        private CommerceInstanceContext _commerceInstanceContext;

        public string Name
        {
            get
            {
                return "Fake";
            }
        }

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public FakePaymentProcessor(CommerceInstanceContext commerceInstanceContext)
        {
            _commerceInstanceContext = commerceInstanceContext;
        }

        public ProcessPaymentResult Process(ProcessPaymentRequest request)
        {
            var commerceName = _commerceInstanceContext.CurrentInstance.Name;
            var redirectUrl = Strings.AreaName 
                + "/Home/Gateway?commerceName=" + commerceName
                + "&paymentId=" + request.Payment.Id
                + "&currency=" + request.CurrencyCode
                + "&commerceReturnUrl=" + HttpUtility.UrlEncode(request.ReturnUrl);

            var commerceUrl = ConfigurationManager.AppSettings["CommerceUrl"];

            if (!String.IsNullOrEmpty(commerceUrl))
            {
                redirectUrl = UrlUtility.Combine(commerceUrl, redirectUrl);
            }
            else
            {
                redirectUrl = redirectUrl.ToFullUrl(HttpContextAccessor());
            }

            return ProcessPaymentResult.Pending(redirectUrl, Guid.NewGuid().ToString("N"));
        }

        public PaymentProcessorEditor GetEditor(PaymentMethod paymentMethod)
        {
            return null;
        }
    }
}