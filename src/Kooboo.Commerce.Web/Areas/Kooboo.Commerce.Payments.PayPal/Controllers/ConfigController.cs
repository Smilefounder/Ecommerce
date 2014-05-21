using Kooboo.CMS.Common;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Controllers;
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
            var settings = PayPalConfig.Deserialize(method.PaymentProcessorData);
            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, HandleAjaxError, Transactional]
        public void Save(int methodId, PayPalConfig model)
        {
            var method = _paymentMethodService.GetById(methodId);
            method.PaymentProcessorData = model.Serialize();
        }
    }
}