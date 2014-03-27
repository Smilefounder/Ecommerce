using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.iDeal.Controllers
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
            var settings = IDealSettings.Deserialize(method.PaymentProcessorData);
            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Settings(int methodId, IDealSettings model, string @return)
        {
            var method = _paymentMethodService.GetById(methodId);
            method.PaymentProcessorData = model.Serialize();
            return AjaxForm().RedirectTo(@return);
        }
    }
}
