using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Settings.Services;
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
        private IKeyValueService _keyValueService;

        public string Name
        {
            get { return Strings.PaymentProcessorName; }
        }

        public BuckarooPaymentProcessor(IKeyValueService keyValueService)
        {
            _keyValueService = keyValueService;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            var settings = BuckarooSettings.FetchFrom(_keyValueService);
            if (settings == null)
                throw new InvalidOperationException("Buckaroo processor should be configured first.");

            var methodId = request.PaymentMethod.PaymentProcessorMethodId;

            var parameters = new NameValueCollection();

            parameters.Add("Brq_websitekey", settings.WebsiteKey);
            parameters.Add("Brq_amount", request.Amount.ToString("f2", CultureInfo.InvariantCulture));
            parameters.Add("Brq_invoicenumber", request.Order.Id.ToString());
            parameters.Add("Brq_currency", request.CurrencyCode);
            parameters.Add("Brq_description", "#" + request.Order.Id.ToString());
            parameters.Add("brq_culture", "en-US");

            parameters.Add("Brq_return", GetCallbackUrl("Return", request));
            parameters.Add("Brq_push", GetCallbackUrl("Push", request));

            parameters.Add("Brq_payment_method", methodId);
            parameters.Add("Brq_service_" + methodId + "_action", "Pay");

            parameters.Add("add_orderid", request.Order.Id.ToString());

            if (methodId == "simplesepadirectdebit")
            {
                parameters.Add("brq_service_" + methodId + "_customeraccountname", request.BankAccountInfo.HolderName);
                parameters.Add("brq_service_" + methodId + "_CustomerBIC", request.BankAccountInfo.BIC);
                parameters.Add("brq_service_" + methodId + "_CustomerIBAN", request.BankAccountInfo.IBAN);
                parameters.Add("brq_service_" + methodId + "_MandateReference", settings.CreditDebitMandateReference);
                parameters.Add("brq_service_" + methodId + "_MandateDate", settings.CreditDebitMandateDate);
            }

            parameters.Add("Brq_signature", BuckarooUtil.GetSignature(parameters, settings.SecretKey));

            return ProcessPaymentResult.Pending(
                new ExternalPostResult(BuckarooUtil.GetGatewayUrl(settings.TestMode), "Buckaroo", parameters));
        }

        private string GetCallbackUrl(string action, ProcessPaymentRequest request)
        {
            var url = UrlUtility.Combine(request.CommerceBaseUrl, Strings.AreaName + "/Buckaroo/" + action) + "?commerceName=" + request.CommerceName;
            if (action.StartsWith("return", StringComparison.OrdinalIgnoreCase))
            {
                url += "&commerceReturnUrl=" + HttpUtility.UrlEncode(request.ReturnUrl);
            }

            return url;
        }

        public IEnumerable<PaymentMethodType> SupportedPaymentTypes
        {
            get
            {
                return new PaymentMethodType[] { PaymentMethodType.CreditCard, PaymentMethodType.DirectDebit, PaymentMethodType.ExternalPayment };
            }
        }

        public bool SupportMultiplePaymentMethods
        {
            get { return true; }
        }

        public IEnumerable<SupportedPaymentMethod> GetSupportedPaymentMethods(PaymentMethodType paymentType)
        {
            if (paymentType == PaymentMethodType.CreditCard)
            {
                return new SupportedPaymentMethod[] {
                    new SupportedPaymentMethod("amex", "American Express"),
                    new SupportedPaymentMethod("visa", "Visa"),
                    new SupportedPaymentMethod("mastercard", "MasterCard")
                };
            };

            if (paymentType == PaymentMethodType.DirectDebit)
            {
                return new SupportedPaymentMethod[] {
                    new SupportedPaymentMethod("simplesepadirectdebit", "SEPA Direct Debit")
                };
            }

            if (paymentType == PaymentMethodType.ExternalPayment)
            {
                return new SupportedPaymentMethod[] {
                    new SupportedPaymentMethod("ideal", "iDeal"),
                    new SupportedPaymentMethod("paypal", "PayPal")
                };
            }

            throw new NotSupportedException("Not support payment type: " + paymentType + ".");
        }
    }
}