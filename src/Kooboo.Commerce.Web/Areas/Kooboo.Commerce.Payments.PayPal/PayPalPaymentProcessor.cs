using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Web;
using Kooboo.Web.Url;
using PayPal;
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
    public class PayPalPaymentProcessor : IPaymentProcessor
    {
        private OrderService _orderService;
        private SettingService _settingsService;

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public string Name
        {
            get
            {
                return Strings.ProcessorName;
            }
        }

        public Type ConfigType
        {
            get
            {
                return typeof(PayPalConfig);
            }
        }

        public PayPalPaymentProcessor(
            OrderService orderService,
            SettingService settingService)
        {
            _orderService = orderService;
            _settingsService = settingService;
        }

        public ProcessPaymentResult Process(PaymentProcessingContext context)
        {
            if (String.IsNullOrEmpty(context.CurrencyCode))
            {
                var storeSettings = _settingsService.Get<GlobalSettings>();
                context.CurrencyCode = storeSettings.Currency;
            }

            var settings = context.ProcessorConfig as PayPalConfig;

            var result = CreatePayPalPayment(context, settings);
            var paymentStatus = PaymentStatus.Failed;

            if (result.state == "approved")
            {
                paymentStatus = PaymentStatus.Success;
            }
            else if (result.state == "canceled")
            {
                paymentStatus = PaymentStatus.Cancelled;
            }
            else // expired, failed
            {
                paymentStatus = PaymentStatus.Failed;
            }

            return new ProcessPaymentResult
            {
                PaymentStatus = paymentStatus,
                ThirdPartyTransactionId = result.id
            };
        }

        private PayPalRest.Payment CreatePayPalPayment(PaymentProcessingContext request, PayPalConfig settings)
        {
            var config = new Dictionary<string, string>();
            config.Add("mode", settings.SandboxMode ? "sandbox" : "live");

            var credentials = new OAuthTokenCredential(settings.ClientId, settings.ClientSecret, config);
            var accessToken = credentials.GetAccessToken();
            var payment = new PayPalRest.Payment
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

            return payment.Create(new APIContext(accessToken)
            {
                Config = config
            });
        }

        private Transaction CreateTransaction(PaymentProcessingContext request)
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

        private CreditCard CreateCreditCard(PaymentProcessingContext request)
        {
            var card = new CreditCard
            {
                number = request.Parameters[PayPalConstants.CreditCardNumber],
                type = request.Parameters[PayPalConstants.CreditCardType],
                expire_month = Convert.ToInt32(request.Parameters[PayPalConstants.CreditCardExpireMonth]),
                expire_year = Convert.ToInt32(request.Parameters[PayPalConstants.CreditCardExpireYear]),
                cvv2 = request.Parameters[PayPalConstants.CreditCardCvv2]
            };

            // If the website don't pass in customer info,
            // then we try to fill these info automatically
            var orderId = request.Payment.OrderId;
            var order = _orderService.Find(orderId);

            card.first_name = order.Customer.FirstName;
            card.last_name = order.Customer.LastName;

            // TODO: Add billing address

            return card;
        }
    }
}