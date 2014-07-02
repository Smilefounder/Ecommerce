using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;
using Kooboo.Commerce.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class RuleController : CommerceControllerBase
    {
        private IActivityProvider _activityProvider;

        public RuleController(IActivityProvider activityProvider)
        {
            _activityProvider = activityProvider;
        }

        public ActionResult Index(string eventName)
        {
            var eventEntry = ActivityEventManager.Instance.FindEvent(eventName);
            var availableActivities = _activityProvider.FindBindableTo(eventEntry.EventType)
                                                       .Select(x => new
                                                       {
                                                           x.Name,
                                                           x.DisplayName
                                                       })
                                                       .ToList();

            ViewBag.Event = eventEntry;
            ViewBag.AvailableActivities = availableActivities;

            var manager = RuleManager.GetManager(CurrentInstance.Name);
            var rules = manager.GetRules(eventName);

            var models = new List<RuleModelBase>();
            foreach (var rule in rules)
            {
                models.Add(RuleModelBase.CreateFrom(rule));
            }

            return View(models);
        }
    }
}
