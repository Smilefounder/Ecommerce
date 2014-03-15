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
        private IKeyValueService _keyValueService;

        public HomeController(IKeyValueService keyValueService)
        {
            _keyValueService = keyValueService;
        }

        public ActionResult Settings(string commerceName)
        {
            var settings = IDealSettings.FetchFrom(_keyValueService);
            return View(settings);
        }

        private string GetFullUrl(string url)
        {
            return UrlUtility.Combine(Request.Url.Scheme + "://" + Request.Url.Authority, url);
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Settings(IDealSettings model, string @return)
        {
            model.SaveTo(_keyValueService);
            return AjaxForm().RedirectTo(@return);
        }
    }
}
