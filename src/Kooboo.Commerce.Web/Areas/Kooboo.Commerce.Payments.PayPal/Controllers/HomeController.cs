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

        public HomeController(IKeyValueService keyValueService)
        {
            _keyValueService = keyValueService;
        }

        public ActionResult Settings()
        {
            var settings = PayPalSettings.FetchFrom(_keyValueService) ?? new PayPalSettings();
            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Settings(PayPalSettings model, string @return)
        {
            model.SaveTo(_keyValueService);
            return AjaxForm().RedirectTo(@return);
        }
    }
}