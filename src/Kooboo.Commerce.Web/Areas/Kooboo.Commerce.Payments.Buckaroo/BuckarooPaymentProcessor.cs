using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
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
    public class BuckarooPaymentProcessor : IPaymentProcessor
    {
        private CommerceInstanceContext _commerceInstanceContext;

        public string Name
        {
            get { return Strings.ProcessorName; }
        }

        public Type ConfigModelType
        {
            get
            {
                return typeof(BuckarooConfig);
            }
        }

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public BuckarooPaymentProcessor(
            IPaymentMethodService paymentMethodService,
            CommerceInstanceContext commerceInstanceContext)
        {
            _commerceInstanceContext = commerceInstanceContext;
        }

        public ProcessPaymentResult Process(PaymentProcessingContext context)
        {
            var settings = context.ProcessorConfig as BuckarooConfig;

            var serviceId = context.Parameters[BuckarooConstants.Parameters.ServiceId];

            var parameters = new NameValueCollection();

            parameters.Add("Brq_websitekey", settings.WebsiteKey);
            parameters.Add("Brq_amount", context.Amount.ToString("f2", CultureInfo.InvariantCulture));
            parameters.Add("Brq_invoicenumber", context.Payment.Id.ToString());
            parameters.Add("Brq_currency", context.CurrencyCode);
            parameters.Add("Brq_description", "#" + context.Payment.Description);
            parameters.Add("brq_culture", "en-US");

            parameters.Add("Brq_return", GetCallbackUrl("Return", context));
            parameters.Add("Brq_push", GetCallbackUrl("Push", context));

            parameters.Add("Brq_payment_method", serviceId);
            parameters.Add("Brq_service_" + serviceId + "_action", "Pay");

            parameters.Add("add_paymentId", context.Payment.Id.ToString());

            if (serviceId == BuckarooConstants.Services.SimpleSEPADirectDebit)
            {
                parameters.Add("brq_service_" + serviceId + "_customeraccountname", context.Parameters[BuckarooConstants.Parameters.SEPA_CustomerAccountName]);
                parameters.Add("brq_service_" + serviceId + "_CustomerBIC", context.Parameters[BuckarooConstants.Parameters.SEPA_CustomerBIC]);
                parameters.Add("brq_service_" + serviceId + "_CustomerIBAN", context.Parameters[BuckarooConstants.Parameters.SEPA_CustomerIBAN]);
                parameters.Add("brq_service_" + serviceId + "_MandateReference", settings.CreditDebitMandateReference);
                parameters.Add("brq_service_" + serviceId + "_MandateDate", settings.CreditDebitMandateDate);
            }

            parameters.Add("Brq_signature", BuckarooUtil.GetSignature(parameters, settings.SecretKey));

            // TODO: Check documentation and see how to do this without redirect
            throw new NotImplementedException();
        }

        private string GetCallbackUrl(string action, PaymentProcessingContext request)
        {
            var commerceName = _commerceInstanceContext.CurrentInstance.Name;
            var url = Strings.AreaName + "/Buckaroo/" + action + "?commerceName=" + commerceName;
            if (action.StartsWith("return", StringComparison.OrdinalIgnoreCase))
            {
                url += "&commerceReturnUrl=" + HttpUtility.UrlEncode(request.ReturnUrl);
            }

            return url.ToFullUrl(HttpContextAccessor());
        }
    }
}