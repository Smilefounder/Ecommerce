using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Activities.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Activities.OrderInvoiceMailing.Controllers
{
    public class HomeController : CommerceControllerBase
    {
        [Inject]
        public IActivityBindingService BindingService { get; set; }

        public ActionResult Settings(int bindingId)
        {
            var binding = BindingService.GetById(bindingId);
            var settings = new ActivityData();

            if (!String.IsNullOrEmpty(binding.ActivityData))
            {
                settings = JsonConvert.DeserializeObject<ActivityData>(binding.ActivityData);
            }

            ViewBag.ActivityBinding = binding;

            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(int bindingId, ActivityData settings, string @return)
        {
            var binding = BindingService.GetById(bindingId);
            binding.ActivityData = JsonConvert.SerializeObject(settings);

            return AjaxForm().RedirectTo(@return);
        }
    }
}
