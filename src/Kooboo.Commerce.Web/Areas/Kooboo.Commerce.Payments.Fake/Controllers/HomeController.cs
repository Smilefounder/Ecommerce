using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Fake.Models;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Web;
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
        private IPaymentService _paymentService;
        private IPaymentMethodService _paymentMethodService;

        public HomeController(
            IPaymentService paymentService,
            IPaymentMethodService paymentMethodService)
        {
            _paymentService = paymentService;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Gateway(int paymentId, string currency, string commerceReturnUrl)
        {
            var payment = _paymentService.GetById(paymentId);

            var model = new GatewayViewModel
            {
                Description = payment.Description,
                PaymentId = paymentId,
                Amount = payment.Amount,
                Currency = currency,
                CommerceReturnUrl = commerceReturnUrl
            };

            return View(model);
        }

        [AutoDbCommit]
        public ActionResult FakeSuccess(int paymentId, string commerceReturnUrl)
        {
            var payment = _paymentService.GetById(paymentId);
            _paymentService.HandlePaymentResult(payment, ProcessPaymentResult.Success(Guid.NewGuid().ToString("N")));
            return Redirect(Url.Payment().DecorateReturn(commerceReturnUrl, payment));
        }

        [AutoDbCommit]
        public ActionResult FakeFailure(int paymentId, string commerceReturnUrl)
        {
            var payment = _paymentService.GetById(paymentId);
            _paymentService.HandlePaymentResult(payment, ProcessPaymentResult.Failed("Payment failed.", Guid.NewGuid().ToString("N")));
            return Redirect(Url.Payment().DecorateReturn(commerceReturnUrl, payment));
        }
    }
}
