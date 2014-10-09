using AuthorizeNet;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.AuthorizeNet
{
    public class AuthorizeNetPaymentProcessor : IPaymentProcessor
    {
        public string Name
        {
            get { return Strings.ProcessorName; }
        }

        public Type ConfigType
        {
            get
            {
                return typeof(AuthorizeNetConfig);
            }
        }

        public PaymentProcessResult Process(PaymentProcessingContext context)
        {
            var settings = context.ProcessorConfig as AuthorizeNetConfig;

            var authRequest = CreateGatewayRequest(settings, context);
            var gateway = new Gateway(settings.LoginId, settings.TransactionKey, settings.SandboxMode);
            var response = gateway.Send(authRequest, context.Payment.Description);

            var result = new PaymentProcessResult();

            if (response.Approved)
            {
                result.PaymentStatus = PaymentStatus.Success;
            }
            else
            {
                result.PaymentStatus = PaymentStatus.Failed;
                result.Message = response.ResponseCode + ": " + response.Message;
            }

            result.TransactionId = response.TransactionID;

            return result;
        }

        private GatewayRequest CreateGatewayRequest(AuthorizeNetConfig settings, PaymentProcessingContext paymentRequest)
        {
            var request = new CardPresentAuthorizeAndCaptureRequest(
                    paymentRequest.Amount,
                    paymentRequest.Parameters[AuthorizeNetConstants.CreditCardNumber],
                    paymentRequest.Parameters[AuthorizeNetConstants.CreditCardExpireMonth],
                    paymentRequest.Parameters[AuthorizeNetConstants.CreditCardExpireYear]
            );

            request.AddCardCode(paymentRequest.Parameters[AuthorizeNetConstants.CreditCardCvv2]);

            return request;
        }
    }
}