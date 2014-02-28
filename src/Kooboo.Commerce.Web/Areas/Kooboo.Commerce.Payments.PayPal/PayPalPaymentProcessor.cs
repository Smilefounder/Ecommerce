using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Web.Url;
using PayPal;
using PayPal.AdaptivePayments;
using PayPal.AdaptivePayments.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.PayPal
{
    [Dependency(typeof(IPaymentProcessor), Key = "Kooboo.Commerce.Payments.PayPal.PayPalPaymentProcessor")]
    public class PayPalPaymentProcessor : IPaymentProcessor
    {
        private IKeyValueService _keyValueService;

        public string Name
        {
            get
            {
                return Strings.PaymentProcessorName;
            }
        }

        public PayPalPaymentProcessor(IKeyValueService keyValueService)
        {
            _keyValueService = keyValueService;
        }

        public IEnumerable<PaymentType> GetSupportedPaymentTypes()
        {
            yield return PaymentType.ExternalPayment;
        }

        public bool SupportMultiplePaymentMethods
        {
            get { return false; }
        }

        public IEnumerable<SupportedPaymentMethod> GetSupportedPaymentMethods(PaymentType paymentType)
        {
            throw new NotSupportedException();
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            var data = PayPalSettings.FetchFrom(_keyValueService);
            var payKey = GetPayKey(request, data);
            var gatewayUrl = PayPalUtil.GetRedirectUrl(payKey, data.SandboxMode);

            return ProcessPaymentResult.Pending(null, gatewayUrl);
        }

        private string GetPayKey(ProcessPaymentRequest request, PayPalSettings data)
        {
            var payRequest = CreatePayRequest(data, request);
            var service = new AdaptivePaymentsService(PayPalUtil.GetPayPalConfig(data));
            var response = service.Pay(payRequest);

            if ((response.responseEnvelope.ack != AckCode.FAILURE) 
                && (response.responseEnvelope.ack != AckCode.FAILUREWITHWARNING))
            {
                return response.payKey;
            }

            throw new PaymentProcessorException(String.Join(Environment.NewLine, response.error.Select(x => x.message)));
        }

        private PayRequest CreatePayRequest(PayPalSettings data, ProcessPaymentRequest request)
        {
            var envelop = new RequestEnvelope("en_US");
            var receivers = new List<Receiver>();
            receivers.Add(new Receiver(request.Amount)
            {
                email = data.MerchantAccount
            });

            var payRequest = new PayRequest(envelop, "PAY", GetCancelUrl(request), request.CurrencyCode, new ReceiverList(receivers), GetReturnUrl(request));
            payRequest.trackingId = request.Order.Id.ToString();
            payRequest.ipnNotificationUrl = GetIPNHandlerUrl(request);

            return payRequest;
        }

        private string GetReturnUrl(ProcessPaymentRequest request)
        {
            return UrlUtility.Combine(request.CommerceBaseUrl,
                Strings.AreaName + "/PayPal/Return?commerceName=" + request.CommerceName
                + "&commerceReturnUrl=" + HttpUtility.UrlEncode(request.ReturnUrl));
        }

        private string GetCancelUrl(ProcessPaymentRequest request)
        {
            return UrlUtility.Combine(request.CommerceBaseUrl,
                Strings.AreaName + "/PayPal/Cancel?commerceName=" + request.CommerceName
                + "&commerceReturnUrl=" + HttpUtility.UrlEncode(request.ReturnUrl));
        }

        private string GetIPNHandlerUrl(ProcessPaymentRequest request)
        {
            return UrlUtility.Combine(request.CommerceBaseUrl,
                Strings.AreaName + "/PayPal/IPN?commerceName=" + request.CommerceName);
        }
    }
}