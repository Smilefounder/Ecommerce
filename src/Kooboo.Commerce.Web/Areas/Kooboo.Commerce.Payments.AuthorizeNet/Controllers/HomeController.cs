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
        private IKeyValueService _keyValueService;

        public HomeController(IKeyValueService keyValueService)
        {
            _keyValueService = keyValueService;
        }

        public ActionResult Settings()
        {
            var settings = AuthorizeNetSettings.FetchFrom(_keyValueService) ?? new AuthorizeNetSettings();
            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Settings(AuthorizeNetSettings settings, string @return)
        {
            settings.SaveTo(_keyValueService);
            return AjaxForm().RedirectTo(@return);
        }
    }
}
