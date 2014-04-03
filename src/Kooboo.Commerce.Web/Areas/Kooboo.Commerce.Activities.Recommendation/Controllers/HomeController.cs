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
        private IActivityRuleService _ruleService;

        public HomeController(IActivityRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        public ActionResult Settings(int ruleId, int attachedActivityId)
        {
            var rule = _ruleService.GetById(ruleId);
            var attachedActivity = rule.AttachedActivities.ById(attachedActivityId);
            var settings = RecommendationActivitySettings.Deserialize(attachedActivity.ActivityData) ?? new RecommendationActivitySettings();

            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(int ruleId, int attachedActivityId, RecommendationActivitySettings settings)
        {
            var rule = _ruleService.GetById(ruleId);
            var attachedActivity = rule.AttachedActivities.ById(attachedActivityId);
            attachedActivity.ActivityData = settings.Serialize();

            return AjaxForm();
        }
    }
}
