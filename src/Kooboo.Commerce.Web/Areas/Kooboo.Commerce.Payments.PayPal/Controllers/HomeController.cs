using Kooboo.CMS.Common;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
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
        private IKeyValueService _keyValueService;
        private IPaymentMethodService _paymentMethodService;

        public HomeController(
            IKeyValueService keyValueService,
            IPaymentMethodService paymentMethodService)
        {
            _keyValueService = keyValueService;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Settings(int methodId)
        {
            var method = _paymentMethodService.GetById(methodId);
            var settings = PayPalSettings.FetchFrom(_keyValueService) ?? new PayPalSettings();

            ViewBag.PaymentMethod = method;

            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(int methodId, PayPalSettings model, string @return)
        {
            var method = _paymentMethodService.GetById(methodId);
            model.SaveTo(_keyValueService);

            return AjaxForm().RedirectTo(@return);
        }
    }
}