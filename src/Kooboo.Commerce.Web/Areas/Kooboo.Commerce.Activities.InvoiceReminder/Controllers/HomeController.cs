using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Activities.InvoiceReminder.Controllers
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
            var settings = new InvoiceReminderSettings();

            if (!String.IsNullOrEmpty(attachedActivity.ActivityData))
            {
                settings = JsonConvert.DeserializeObject<InvoiceReminderSettings>(attachedActivity.ActivityData);
            }

            return View(settings);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Settings(int ruleId, int attachedActivityId, InvoiceReminderSettings settings)
        {
            var rule = _ruleService.GetById(ruleId);
            var attachedActivity = rule.AttachedActivities.ById(attachedActivityId);
            attachedActivity.ActivityData = JsonConvert.SerializeObject(settings);

            return AjaxForm();
        }
    }
}
