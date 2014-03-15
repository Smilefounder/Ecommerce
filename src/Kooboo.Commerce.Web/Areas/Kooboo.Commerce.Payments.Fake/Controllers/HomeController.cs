using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Fake.Models;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.Fake.Controllers
{
    public class HomeController : CommerceControllerBase
    {
        private IOrderService _orderService;
        private IOrderPaymentService _orderPaymentService;
        private IPaymentMethodService _paymentMethodService;

        public HomeController(
            IOrderService orderService,
            IOrderPaymentService orderPaymentService,
            IPaymentMethodService paymentMethodService)
        {
            _orderService = orderService;
            _orderPaymentService = orderPaymentService;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Gateway(int orderId, decimal amount, string currency, string commerceReturnUrl)
        {
            var model = new GatewayViewModel
            {
                OrderId = orderId,
                Amount = amount,
                Currency = currency,
                CommerceReturnUrl = commerceReturnUrl
            };

            return View(model);
        }

        [AutoDbCommit]
        public ActionResult FakeSuccess(int orderId, string commerceReturnUrl)
        {
            var order = _orderService.GetById(orderId);
            _orderPaymentService.HandlePaymentResult(order, ProcessPaymentResult.Success(Guid.NewGuid().ToString("N")));

            return Redirect(PaymentReturnUrlUtil.AppendOrderInfoToQueryString(commerceReturnUrl, _orderService.GetById(orderId)));
        }

        [AutoDbCommit]
        public ActionResult FakeFailure(int orderId, string commerceReturnUrl)
        {
            var order = _orderService.GetById(orderId);
            _orderPaymentService.HandlePaymentResult(order, ProcessPaymentResult.Failed("Payment failed.", Guid.NewGuid().ToString("N")));

            return Redirect(PaymentReturnUrlUtil.AppendOrderInfoToQueryString(commerceReturnUrl, _orderService.GetById(orderId)));
        }
    }
}
