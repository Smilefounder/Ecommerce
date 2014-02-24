using Kooboo.Commerce.Activities.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Activities.Recommendation.Controllers
{
    public class HomeController : CommerceControllerBase
    {
        private IActivityBindingService _bindingService;

        public HomeController(IActivityBindingService bindingService)
        {
            _bindingService = bindingService;
        }

        public ActionResult Settings(int bindingId)
        {
            var binding = _bindingService.GetById(bindingId);
            var settings = RecommendationActivitySettings.Deserialize(binding.ActivityData) ?? new RecommendationActivitySettings();

            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(int bindingId, RecommendationActivitySettings settings, string @return)
        {
            var binding = _bindingService.GetById(bindingId);
            binding.ActivityData = settings.Serialize();

            return AjaxForm().RedirectTo(@return);
        }
    }
}
