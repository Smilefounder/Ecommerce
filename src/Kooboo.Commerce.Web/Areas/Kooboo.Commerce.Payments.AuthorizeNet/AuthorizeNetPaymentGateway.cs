using AuthorizeNet;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.AuthorizeNet
{
    [Dependency(typeof(IPaymentGateway), Key = "Kooboo.Commerce.Payments.AuthorizeNetPaymentGateway")]
    public class AuthorizeNetPaymentGateway : IPaymentGateway
    {
        public string Name
        {
            get { return Strings.PaymentGatewayName; }
        }

        public PaymentGatewayType PaymentGatewayType
        {
            get { return PaymentGatewayType.DirectCreditCardPayment; }
        }

        public bool SupportBankSelection
        {
            get { return false; }
        }

        public IEnumerable<BankInfo> GetSupportedBanks(PaymentMethod method)
        {
            throw new NotSupportedException();
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            if (request.CreditCardInfo == null)
                throw new InvalidOperationException("Require credit card info.");

            var settings = AuthorizeNetSettings.Deserialize(request.PaymentMethod.PaymentGatewayData);
            if (settings == null)
                throw new InvalidOperationException("Missing payment gateway configuration.");

            var authRequest = CreateGatewayRequest(settings, request);
            var gateway = new Gateway(settings.LoginId, settings.TransactionKey, settings.SandboxMode);
            var response = gateway.Send(authRequest, "Order #" + request.Order.Id);

            var result = new ProcessPaymentResult();

            if (response.Approved)
            {
                result.AuthorizationTransactionCode = response.AuthorizationCode;
                result.PaymentStatus = PaymentStatus.Success;
            }
            else
            {
                result.PaymentStatus = PaymentStatus.Failed;
                result.ErrorMessage = response.ResponseCode + ": " + response.Message;
            }

            result.AuthorizationTransactionId = response.TransactionID;

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
    }
}