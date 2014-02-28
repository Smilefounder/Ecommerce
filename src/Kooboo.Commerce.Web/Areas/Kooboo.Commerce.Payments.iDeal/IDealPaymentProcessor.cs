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

        public string Name
        {
            get
            {
                return Strings.PaymentProcessorName;
            }
        }

        public IDealPaymentProcessor(IKeyValueService keyValueService)
        {
            _keyValueService = keyValueService;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            if (request.Amount < (decimal)1.19)
                throw new FormatException("Amount cannot be less than € 1,19");

            var settings = IDealSettings.FetchFrom(_keyValueService);

            var idealFetch = new IdealFetch(
                settings.PartnerId
                , "#" + request.Order.Id
                , UrlUtility.Combine(request.CommerceBaseUrl, Strings.AreaName + "/iDeal/Callback?commerceName=" + request.CommerceName)
                , UrlUtility.Combine(request.CommerceBaseUrl, Strings.AreaName + "/iDeal/Return?commerceName=" + request.CommerceName + "&commerceReturnUrl=" + HttpUtility.UrlEncode(request.ReturnUrl))
                , request.BankId
                , request.Amount
            );

            if (idealFetch.Error)
                throw new PaymentProcessorException(idealFetch.ErrorMessage);

            return ProcessPaymentResult.Pending(idealFetch.TransactionId, idealFetch.Url);
        }

        public IEnumerable<PaymentType> SupportedPaymentTypes
        {
            get
            {
                yield return PaymentType.ExternalPayment;
            }
        }

        public bool SupportMultiplePaymentMethods
        {
            get { return false; }
        }

        public IEnumerable<SupportedPaymentMethod> GetSupportedPaymentMethods(PaymentType paymentType)
        {
            throw new NotSupportedException();
        }
    }
}