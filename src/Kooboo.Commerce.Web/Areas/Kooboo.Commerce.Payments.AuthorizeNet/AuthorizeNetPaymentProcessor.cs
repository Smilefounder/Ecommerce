using AuthorizeNet;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Settings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.AuthorizeNet
{
    [Dependency(typeof(IPaymentProcessor), Key = "Kooboo.Commerce.Payments.AuthorizeNetPaymentProcessor")]
    public class AuthorizeNetPaymentProcessor : IPaymentProcessor
    {
        private IKeyValueService _keyValueService;

        public string Name
        {
            get { return Strings.PaymentGatewayName; }
        }

        public AuthorizeNetPaymentProcessor(IKeyValueService keyValueService)
        {
            _keyValueService = keyValueService;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            if (request.CreditCardInfo == null)
                throw new InvalidOperationException("Require credit card info.");

            var settings = AuthorizeNetSettings.FetchFrom(_keyValueService);
            if (settings == null)
                throw new InvalidOperationException("Missing payment gateway configuration.");

            var authRequest = CreateGatewayRequest(settings, request);
            var gateway = new Gateway(settings.LoginId, settings.TransactionKey, settings.SandboxMode);
            var response = gateway.Send(authRequest, request.Payment.Description);

            var result = new ProcessPaymentResult();

            if (response.Approved)
            {
                result.PaymentStatus = PaymentStatus.Success;
            }
            else
            {
                result.PaymentStatus = PaymentStatus.Failed;
                result.ErrorMessage = response.ResponseCode + ": " + response.Message;
            }

            result.PaymentTransactionId = response.TransactionID;

            return result;
        }

        private GatewayRequest CreateGatewayRequest(AuthorizeNetSettings settings, ProcessPaymentRequest paymentRequest)
        {
            var request = new CardPresentAuthorizeAndCaptureRequest(
                    paymentRequest.Amount,
                    paymentRequest.CreditCardInfo.CardNumber,
                    paymentRequest.CreditCardInfo.ExpiredMonth.ToString("D2"),
                    paymentRequest.CreditCardInfo.ExpiredYear.ToString()
            );

            request.AddCardCode(paymentRequest.CreditCardInfo.Cvv2);

            return request;
        }


        public IEnumerable<PaymentMethodType> SupportedPaymentTypes
        {
            get
            {
                yield return PaymentMethodType.CreditCard;
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