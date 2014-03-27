using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web;
using Kooboo.Web.Url;
using PayPal;
using PayPal.AdaptivePayments;
using PayPal.AdaptivePayments.Model;
using PayPal.Api.Payments;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPalRest = PayPal.Api.Payments;

namespace Kooboo.Commerce.Payments.PayPal
{
    [Dependency(typeof(IPaymentProcessor), Key = "Kooboo.Commerce.Payments.PayPal.PayPalPaymentProcessor")]
    public class PayPalPaymentProcessor : IPaymentProcessor
    {
        private IOrderService _orderService;
        private IPaymentMethodService _paymentMethodService;

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public string Name
        {
            get
            {
                return Strings.PaymentProcessorName;
            }
        }

        public PayPalPaymentProcessor(
            IOrderService orderService,
            IPaymentMethodService paymentMethodService)
        {
            _orderService = orderService;
            _paymentMethodService = paymentMethodService;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            var paymentMethod = _paymentMethodService.GetById(request.Payment.PaymentMethod.Id);
            var settings = PayPalSettings.Deserialize(paymentMethod.PaymentProcessorData);

            var accessToken = new OAuthTokenCredential(settings.ClientId, settings.ClientSecret).GetAccessToken();
            var paypalPayment = new PayPalRest.Payment
            {
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "credit_card",
                    funding_instruments = new List<PayPalRest.FundingInstrument>
                    {
                        new PayPalRest.FundingInstrument
                        {
                            credit_card = CreateCreditCard(request)
                        }
                    }
                },
                transactions = new List<Transaction> { CreateTransaction(request) }
            };

            paypalPayment.Create(accessToken);

            var paymentStatus = PaymentStatus.Failed;

            if (paypalPayment.state == "approved")
            {
                paymentStatus = PaymentStatus.Success;
            }
            else if (paypalPayment.state == "canceled")
            {
                paymentStatus = PaymentStatus.Cancelled;
            }
            else // expired, failed
            {
                paymentStatus = PaymentStatus.Failed;
            }

            var paypalPaymentId = paypalPayment.id;

            return new ProcessPaymentResult
            {
                PaymentStatus = paymentStatus,
                ThirdPartyTransactionId = paypalPaymentId
            };
        }

        private Transaction CreateTransaction(ProcessPaymentRequest request)
        {
            return new Transaction
            {
                amount = new Amount
                {
                    total = request.Payment.Amount.ToString("f2", CultureInfo.InvariantCulture),
                    currency = request.CurrencyCode
                },
                description = request.Payment.Description
            };
        }

        private CreditCard CreateCreditCard(ProcessPaymentRequest request)
        {
            var card = new CreditCard
            {
                number = request.Parameters[PayPalParameters.CreditCardNumber],
                type = request.Parameters[PayPalParameters.CreditCardType],
                expire_month = Convert.ToInt32(request.Parameters[PayPalParameters.CreditCardExpireMonth]),
                expire_year = Convert.ToInt32(request.Parameters[PayPalParameters.CreditCardExpireYear]),
                cvv2 = request.Parameters[PayPalParameters.CreditCardCvv2]
            };

            // If the website don't pass in customer info,
            // then we try to fill these info automatically
            if (request.Payment.PaymentTargetType == PaymentTargetTypes.Order)
            {
                var orderId = Convert.ToInt32(request.Payment.PaymentTargetId);
                var order = _orderService.GetById(orderId);

                card.first_name = order.Customer.FirstName;
                card.last_name = order.Customer.LastName;

                // TODO: Add billing address
            }

            return card;
        }
    }
}