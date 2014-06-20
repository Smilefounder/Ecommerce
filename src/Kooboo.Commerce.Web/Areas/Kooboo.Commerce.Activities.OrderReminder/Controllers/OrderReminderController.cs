using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Activities.OrderReminder.Controllers
{
    public class OrderReminderController : CommerceControllerBase
    {
        private IRepository<ActivityRule> _ruleRepository;

        public OrderReminderController(IRepository<ActivityRule> ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public ActionResult GetConfig(int ruleId, int attachedActivityInfoId)
        {
            var rule = _ruleRepository.Get(ruleId);
            var attachedActivity = rule.AttachedActivityInfos.Find(attachedActivityInfoId);
            var config = attachedActivity.LoadParameters(typeof(OrderReminderActivityConfig));
            return JsonNet(config).UsingClientConvention();
        }

        [HttpPost, Transactional, HandleAjaxError]
        public void SaveConfig(int ruleId, int attachedActivityInfoId, OrderReminderActivityConfig config)
        {
            var rule = _ruleRepository.Get(ruleId);
            var attachedActivity = rule.AttachedActivityInfos.Find(attachedActivityInfoId);
            attachedActivity.UpdateParameters(config);
        }
    }
}
