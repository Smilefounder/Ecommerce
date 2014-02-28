using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Payment;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class PaymentController : CommerceControllerBase
    {
        private ISettingService _settingService;
        private IOrderService _orderService;
        private IOrderPaymentService _orderPaymentService;
        private IPaymentMethodService _paymentMethodService;
        private IPaymentProcessorFactory _paymentGatewayFactory;

        public PaymentController(
            ISettingService settingService,
            IOrderService orderService,
            IOrderPaymentService orderPaymentService,
            IPaymentMethodService paymentMethodService,
            IPaymentProcessorFactory paymentGatewayFactory)
        {
            _settingService = settingService;
            _orderService = orderService;
            _orderPaymentService = orderPaymentService;
            _paymentMethodService = paymentMethodService;
            _paymentGatewayFactory = paymentGatewayFactory;
        }

        [Transactional]
        public ActionResult Gateway(PaymentRequestModel model)
        {
            var settings = _settingService.GetStoreSetting();
            var order = _orderService.GetById(model.OrderId);
            var paymentMethod = _paymentMethodService.GetById(model.PaymentMethodId);
            var commerceName = CommerceContext.CurrentInstance.Name;

            var paymentRequest = new ProcessPaymentRequest(commerceName, order, paymentMethod)
            {
                CurrencyCode = settings.CurrencyISOCode,
                BankId = model.BankId,
                CreditCardInfo = model.CreditCardInfo,
                CommerceBaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority,
                ReturnUrl = model.ReturnUrl
            };

            var gateway = _paymentGatewayFactory.FindByName(paymentMethod.PaymentProcessor);
            var result = gateway.ProcessPayment(paymentRequest);

            if (result.PaymentStatus == PaymentStatus.Success)
            {
                // Already done
                _orderPaymentService.HandlePaymentResult(order, result);

                return Redirect(paymentRequest.ReturnUrl);
            }
            else
            {
                return Redirect(result.RedirectUrl);
            }
        }
    }
}