using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Settings;
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
    public class IDealPaymentProcessor : IPaymentProcessor
    {
        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public string Name
        {
            get
            {
                return "iDeal";
            }
        }

        public Type ConfigType
        {
            get
            {
                return typeof(IDealConfig);
            }
        }

        public PaymentProcessResult Process(PaymentProcessingContext context)
        {
            if (context.Amount < (decimal)1.19)
                throw new FormatException("Amount cannot be less than € 1,19");

            var settings = context.ProcessorConfig as IDealConfig;

            var instance = CommerceInstance.Current.Name;
            var httpContext = HttpContextAccessor();
            var reportUrl = Strings.AreaName + "/iDeal/Callback?instance=" + instance;
            var returnUrl = Strings.AreaName
                + "/iDeal/Return?instance=" + instance
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

            return PaymentProcessResult.Pending(idealFetch.Url, idealFetch.TransactionId);
        }
    }
}