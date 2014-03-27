using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.Buckaroo
{
    [Dependency(typeof(IPaymentProcessor), Key = "Kooboo.Commerce.Payments.Buckaroo.BuckarooPaymentProcessor")]
    public class BuckarooPaymentProcessor : IPaymentProcessor
    {
        private IPaymentMethodService _paymentMethodService;

        public string Name
        {
            get { return Strings.PaymentProcessorName; }
        }

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public BuckarooPaymentProcessor(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            var method = _paymentMethodService.GetById(request.Payment.PaymentMethod.Id);
            var settings = BuckarooSettings.Deserialize(method.PaymentProcessorData);

            var serviceId = request.Parameters[BuckarooConstants.Parameters.ServiceId];

            var parameters = new NameValueCollection();

            parameters.Add("Brq_websitekey", settings.WebsiteKey);
            parameters.Add("Brq_amount", request.Amount.ToString("f2", CultureInfo.InvariantCulture));
            parameters.Add("Brq_invoicenumber", request.Payment.Id.ToString());
            parameters.Add("Brq_currency", request.CurrencyCode);
            parameters.Add("Brq_description", "#" + request.Payment.Description);
            parameters.Add("brq_culture", "en-US");

            parameters.Add("Brq_return", GetCallbackUrl("Return", request));
            parameters.Add("Brq_push", GetCallbackUrl("Push", request));

            parameters.Add("Brq_payment_method", serviceId);
            parameters.Add("Brq_service_" + serviceId + "_action", "Pay");

            parameters.Add("add_paymentId", request.Payment.Id.ToString());

            if (serviceId == BuckarooConstants.Services.SimpleSEPADirectDebit)
            {
                parameters.Add("brq_service_" + serviceId + "_customeraccountname", request.Parameters[BuckarooConstants.Parameters.SEPA_CustomerAccountName]);
                parameters.Add("brq_service_" + serviceId + "_CustomerBIC", request.Parameters[BuckarooConstants.Parameters.SEPA_CustomerBIC]);
                parameters.Add("brq_service_" + serviceId + "_CustomerIBAN", request.Parameters[BuckarooConstants.Parameters.SEPA_CustomerIBAN]);
                parameters.Add("brq_service_" + serviceId + "_MandateReference", settings.CreditDebitMandateReference);
                parameters.Add("brq_service_" + serviceId + "_MandateDate", settings.CreditDebitMandateDate);
            }

            parameters.Add("Brq_signature", BuckarooUtil.GetSignature(parameters, settings.SecretKey));

            // TODO: Check documentation and see how to do this without redirect
            throw new NotImplementedException();
        }

        private string GetCallbackUrl(string action, ProcessPaymentRequest request)
        {
            var url = Strings.AreaName + "/Buckaroo/" + action + "?commerceName=" + request.Payment.Metadata.CommerceName;
            if (action.StartsWith("return", StringComparison.OrdinalIgnoreCase))
            {
                url += "&commerceReturnUrl=" + HttpUtility.UrlEncode(request.ReturnUrl);
            }

            return url.ToFullUrl(HttpContextAccessor());
        }
    }
}