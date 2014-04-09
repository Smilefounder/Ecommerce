using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Settings.Services;
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
    [Dependency(typeof(IPaymentProcessor), Key = "Kooboo.Commerce.Payments.PayPal.PayPalPaymentProcessor")]
    public class PayPalPaymentProcessor : IPaymentProcessor
    {
        private IOrderService _orderService;
        private ISettingService _settingsService;
        private IPaymentMethodService _paymentMethodService;

        public Func<HttpContextBase> HttpContextAccessor = () => new HttpContextWrapper(HttpContext.Current);

        public string Name
        {
            get
            {
                return Strings.PaymentProcessorName;
            }
        }

        public IEnumerable<PaymentProcessorParameterDescriptor> ParameterDescriptors
        {
            get
            {
                return PayPalConstants.ParameterDescriptors;
            }
        }

        public PayPalPaymentProcessor(
            IOrderService orderService,
            ISettingService settingService,
            IPaymentMethodService paymentMethodService)
        {
            _orderService = orderService;
            _settingsService = settingService;
            _paymentMethodService = paymentMethodService;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest request)
        {
            if (String.IsNullOrEmpty(request.CurrencyCode))
            {
                var storeSettings = _settingsService.Get<StoreSettings>(StoreSettings.Key) ?? new StoreSettings();
                request.CurrencyCode = storeSettings.CurrencyISOCode;
            }

            var paymentMethod = _paymentMethodService.GetById(request.Payment.PaymentMethod.Id);
            var settings = PayPalSettings.Deserialize(paymentMethod.PaymentProcessorData);

            var result = CreatePayPalPayment(request, settings);
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

        private PayPalRest.Payment CreatePayPalPayment(ProcessPaymentRequest request, PayPalSettings settings)
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
                number = request.Parameters[PayPalConstants.CreditCardNumber],
                type = request.Parameters[PayPalConstants.CreditCardType],
                expire_month = Convert.ToInt32(request.Parameters[PayPalConstants.CreditCardExpireMonth]),
                expire_year = Convert.ToInt32(request.Parameters[PayPalConstants.CreditCardExpireYear]),
                cvv2 = request.Parameters[PayPalConstants.CreditCardCvv2]
            };

            // If the website don't pass in customer info,
            // then we try to fill these info automatically
            if (request.Payment.PaymentTarget.Type == PaymentTargetTypes.Order)
            {
                var orderId = Convert.ToInt32(request.Payment.PaymentTarget.Id);
                var order = _orderService.GetById(orderId);

                card.first_name = order.Customer.FirstName;
                card.last_name = order.Customer.LastName;

                // TODO: Add billing address
            }

            return card;
        }
    }
}