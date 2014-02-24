using Kooboo.CMS.Common;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.PayPal.Controllers
{
    public class HomeController : CommerceControllerBase
    {
        private IPaymentMethodService _paymentMethodService;

        public HomeController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Settings(int methodId)
        {
            var method = _paymentMethodService.GetById(methodId);
            var settings = PayPalPaymentGatewayData.Deserialize(method.PaymentGatewayData) ?? new PayPalPaymentGatewayData();

            ViewBag.PaymentMethod = method;

            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(int methodId, PayPalPaymentGatewayData model, string @return)
        {
            var method = _paymentMethodService.GetById(methodId);
            method.PaymentGatewayData = model.Serialize();

            return AjaxForm().RedirectTo(@return);
        }
    }
}