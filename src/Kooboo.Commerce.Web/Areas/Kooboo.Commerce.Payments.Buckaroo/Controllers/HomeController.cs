using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.Buckaroo.Controllers
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
            var settings = BuckarooSettings.FetchFrom(_keyValueService);
            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(BuckarooSettings settings, string @return)
        {
            settings.SaveTo(_keyValueService);
            return AjaxForm().RedirectTo(@return);
        }
    }
}
