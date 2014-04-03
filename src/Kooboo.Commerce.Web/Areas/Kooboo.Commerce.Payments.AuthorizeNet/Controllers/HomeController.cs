using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.AuthorizeNet.Controllers
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
            var settings = AuthorizeNetSettings.Deserialize(method.PaymentProcessorData);
            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(int methodId, AuthorizeNetSettings settings, string @return)
        {
            var method = _paymentMethodService.GetById(methodId);
            method.PaymentProcessorData = settings.Serialize();
            return AjaxForm().RedirectTo(@return);
        }
    }
}
