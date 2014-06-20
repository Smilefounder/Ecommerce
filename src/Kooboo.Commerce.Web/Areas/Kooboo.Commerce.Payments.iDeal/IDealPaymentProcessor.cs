using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web;
using Kooboo.Web.Url;
using Mollie.iDEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.iDeal
{
    [Dependency(typeof(IPaymentProcessor), Key = "iDeal")]
    public class IDealPaymentProcessor : IPaymentProcessor
    {
        private CommerceInstanceContext _commerceInstanceContext;

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public string Name
        {
            get
            {
                return "iDeal";
            }
        }

        public Type ConfigModelType
        {
            get
            {
                return typeof(IDealConfig);
            }
        }

        public IDealPaymentProcessor(CommerceInstanceContext commerceInstanceContext)
        {
            _commerceInstanceContext = commerceInstanceContext;
        }

        public ProcessPaymentResult Process(PaymentProcessingContext context)
        {
            if (context.Amount < (decimal)1.19)
                throw new FormatException("Amount cannot be less than € 1,19");

            var settings = context.ProcessorConfig as IDealConfig;

            var commerceName = _commerceInstanceContext.CurrentInstance.Name;
            var httpContext = HttpContextAccessor();
            var reportUrl = Strings.AreaName + "/iDeal/Callback?commerceName=" + commerceName;
            var returnUrl = Strings.AreaName
                + "/iDeal/Return?commerceName=" + commerceName
                + "&paymentId=" + context.Payment.Id
                + "&commerceReturnUrl=" + HttpUtility.UrlEncode(context.ReturnUrl);

            var idealFetch = new IdealFetch(
                settings.PartnerId
                , context.Payment.Description
                , reportUrl.ToFullUrl(httpContext)
                , returnUrl.ToFullUrl(httpContext)
                , ""
                , context.Amount
            );

            if (idealFetch.Error)
                throw new PaymentProcessorException(idealFetch.ErrorMessage);

            return ProcessPaymentResult.Pending(idealFetch.Url, idealFetch.TransactionId);
        }
    }
}