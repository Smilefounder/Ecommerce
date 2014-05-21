using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Controllers;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.AuthorizeNet.Controllers
{
    public class ConfigController : CommerceControllerBase
    {
        private IPaymentMethodService _paymentMethodService;

        public ConfigController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Load(int methodId)
        {
            var method = _paymentMethodService.GetById(methodId);
            var settings = AuthorizeNetConfig.Deserialize(method.PaymentProcessorData);
            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, HandleAjaxError, Transactional]
        public void Save(int methodId, AuthorizeNetConfig settings)
        {
            var method = _paymentMethodService.GetById(methodId);
            method.PaymentProcessorData = settings.Serialize();
        }
    }
}
