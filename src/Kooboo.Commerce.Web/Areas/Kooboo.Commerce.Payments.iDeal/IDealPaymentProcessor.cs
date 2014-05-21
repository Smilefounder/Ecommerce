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
        private IPaymentMethodService _paymentMethodService;
        private CommerceInstanceContext _commerceInstanceContext;

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public string Name
        {
            get
            {
                return "iDeal";
            }
        }

        public IDealPaymentProcessor(IPaymentMethodService paymentMethodService, CommerceInstanceContext commerceInstanceContext)
        {
            _paymentMethodService = paymentMethodService;
            _commerceInstanceContext = commerceInstanceContext;
        }

        public ProcessPaymentResult Process(ProcessPaymentRequest request)
        {
            if (request.Amount < (decimal)1.19)
                throw new FormatException("Amount cannot be less than € 1,19");

            var method = _paymentMethodService.GetById(request.Payment.PaymentMethod.Id);
            var settings = IDealConfig.Deserialize(method.PaymentProcessorData);

            var commerceName = _commerceInstanceContext.CurrentInstance.Name;
            var httpContext = HttpContextAccessor();
            var reportUrl = Strings.AreaName + "/iDeal/Callback?commerceName=" + commerceName;
            var returnUrl = Strings.AreaName
                + "/iDeal/Return?commerceName=" + commerceName
                + "&paymentId=" + request.Payment.Id
                + "&commerceReturnUrl=" + HttpUtility.UrlEncode(request.ReturnUrl);

            var idealFetch = new IdealFetch(
                settings.PartnerId
                , request.Payment.Description
                , reportUrl.ToFullUrl(httpContext)
                , returnUrl.ToFullUrl(httpContext)
                , ""
                , request.Amount
            );

            if (idealFetch.Error)
                throw new PaymentProcessorException(idealFetch.ErrorMessage);

            return ProcessPaymentResult.Pending(idealFetch.Url, idealFetch.TransactionId);
        }

        public PaymentProcessorEditor GetEditor()
        {
            return new PaymentProcessorEditor("~/Areas/" + Strings.AreaName + "/Views/Config.cshtml");
        }
    }
}