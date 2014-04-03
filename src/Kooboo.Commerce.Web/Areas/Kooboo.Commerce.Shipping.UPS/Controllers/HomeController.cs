using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping.UPS.Controllers
{
    public class HomeController : CommerceControllerBase
    {
        private ISettingService _settingsService;

        public HomeController(ISettingService settingsService)
        {
            _settingsService = settingsService;
        }

        public ActionResult Settings()
        {
            var settings = UPSSettings.LoadFrom(_settingsService);
            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(UPSSettings model, string @return)
        {
            model.Save(_settingsService);
            return AjaxForm().RedirectTo(@return);
        }
    }
}
