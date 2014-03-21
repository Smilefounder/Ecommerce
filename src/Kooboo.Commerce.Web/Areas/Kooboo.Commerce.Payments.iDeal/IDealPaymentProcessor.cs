using Kooboo.CMS.Common.Runtime.Dependency;
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
    [Dependency(typeof(IPaymentProcessor), Key = "Kooboo.Commerce.Payments.iDeal.IDealPaymentProcessor")]
    public class IDealPaymentProcessor : IPaymentProcessor
    {
        private IKeyValueService _keyValueService;
        private IPaymentMethodService _paymentMethodService;

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public string Name
        {
            get
            {
                return Strings.PaymentProcessorName;
            }
        }

        public IDealPaymentProcessor(IKeyValueService keyValueService, IPaymentMethodService paymentMethodService)
        {
            _keyValueService = keyValueService;
            _paymentMethodService = paymentMethodService;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            if (request.Amount < (decimal)1.19)
                throw new FormatException("Amount cannot be less than € 1,19");

            var settings = IDealSettings.FetchFrom(_keyValueService);
            var method = _paymentMethodService.GetById(request.Payment.PaymentMethod.Id);

            var httpContext = HttpContextAccessor();
            var reportUrl = Strings.AreaName + "/iDeal/Callback?commerceName=" + request.Payment.Metadata.CommerceName;
            var returnUrl = Strings.AreaName
                + "/iDeal/Return?commerceName=" + request.Payment.Metadata.CommerceName 
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

            return ProcessPaymentResult.Pending(new RedirectResult(idealFetch.Url), idealFetch.TransactionId);
        }

        public IEnumerable<PaymentMethodType> SupportedPaymentTypes
        {
            get
            {
                yield return PaymentMethodType.ExternalPayment;
            }
        }

        public bool SupportMultiplePaymentMethods
        {
            get { return false; }
        }

        public IEnumerable<SupportedPaymentMethod> GetSupportedPaymentMethods(PaymentMethodType paymentType)
        {
            throw new NotSupportedException();
        }
    }
}