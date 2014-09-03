using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments.Fake.Models;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Web;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.Fake.Controllers
{
    public class HomeController : CommerceController
    {
        private PaymentService _paymentService;
        private PaymentMethodService _paymentMethodService;

        public HomeController(PaymentService paymentService, PaymentMethodService paymentMethodService)
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

        [Transactional]
        public ActionResult FakeSuccess(int paymentId, string commerceReturnUrl)
        {
            var payment = _paymentService.GetById(paymentId);
            _paymentService.AcceptProcessResult(payment, ProcessPaymentResult.Success(Guid.NewGuid().ToString("N")));
            return Redirect(Url.Payment().DecorateReturn(commerceReturnUrl, payment));
        }

        [Transactional]
        public ActionResult FakeFailure(int paymentId, string commerceReturnUrl)
        {
            var payment = _paymentService.GetById(paymentId);
            _paymentService.AcceptProcessResult(payment, ProcessPaymentResult.Failed("Payment failed.", Guid.NewGuid().ToString("N")));
            return Redirect(Url.Payment().DecorateReturn(commerceReturnUrl, payment));
        }
    }
}
