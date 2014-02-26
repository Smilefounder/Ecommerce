using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Payments.Services;
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
    [Dependency(typeof(IPaymentGateway), Key = "Kooboo.Commerce.Payments.PayPal.PayPalPaymentGateway")]
    public class PayPalPaymentGateway : IPaymentGateway
    {
        public string Name
        {
            get
            {
                return Strings.PaymentGatewayName;
            }
        }

        public bool SupportBankSelection
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<BankInfo> GetSupportedBanks(PaymentMethod method)
        {
            return Enumerable.Empty<BankInfo>();
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            var data = PayPalPaymentGatewayData.Deserialize(request.PaymentMethod.PaymentGatewayData);
            var payKey = GetPayKey(request, data);
            var gatewayUrl = PayPalUtil.GetRedirectUrl(payKey, data.SandboxMode);

            return ProcessPaymentResult.Pending(null, gatewayUrl);
        }

        private string GetPayKey(ProcessPaymentRequest request, PayPalPaymentGatewayData data)
        {
            var payRequest = CreatePayRequest(data, request);
            var service = new AdaptivePaymentsService(PayPalUtil.GetPayPalConfig(data));
            var response = service.Pay(payRequest);

            if ((response.responseEnvelope.ack != AckCode.FAILURE) 
                && (response.responseEnvelope.ack != AckCode.FAILUREWITHWARNING))
            {
                return response.payKey;
            }

            throw new PaymentGatewayException(String.Join(Environment.NewLine, response.error.Select(x => x.message)));
        }

        private PayRequest CreatePayRequest(PayPalPaymentGatewayData data, ProcessPaymentRequest request)
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

        public PaymentGatewayType PaymentGatewayType
        {
            get
            {
                return PaymentGatewayType.RedirectedPayment;
            }
        }
    }
}