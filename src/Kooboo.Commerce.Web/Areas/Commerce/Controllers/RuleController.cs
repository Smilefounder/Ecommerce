using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;
using Kooboo.Commerce.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Extensions;
using Newtonsoft.Json;
using Kooboo.Commerce.Rules.Activities;

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
            ViewBag.EventTypeName = eventEntry.EventType.AssemblyQualifiedNameWithoutVersion();
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

        [HttpPost, HandleAjaxError]
        public void Save(string eventName, string json)
        {
            var models = JsonConvert.DeserializeObject<List<RuleModelBase>>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            var rules = new List<RuleBase>();

            foreach (var model in models)
            {
                rules.Add(CreateRule(model));
            }

            var manager = RuleManager.GetManager(CurrentInstance.Name);
            manager.SaveRules(eventName, rules);
        }

        private RuleBase CreateRule(RuleModelBase model)
        {
            if (model is IfElseRuleModel)
            {
                return CreateIfElseRule(model as IfElseRuleModel);
            }
            if (model is AlwaysRuleModel)
            {
                return CreateAlwaysRule(model as AlwaysRuleModel);
            }

            throw new NotSupportedException();
        }

        private IfElseRule CreateIfElseRule(IfElseRuleModel model)
        {
            var rule = new IfElseRule();
            rule.Conditions = model.Conditions.ToList();

            foreach (var thenRuleModel in model.Then)
            {
                rule.Then.Add(CreateRule(thenRuleModel));
            }

            foreach (var elseRuleModel in model.Else)
            {
                rule.Else.Add(CreateRule(elseRuleModel));
            }

            return rule;
        }

        private AlwaysRule CreateAlwaysRule(AlwaysRuleModel model)
        {
            var rule = new AlwaysRule();
            foreach (var activityModel in model.Activities)
            {
                rule.Activities.Add(CreateConfiguredActivity(activityModel));
            }
            return rule;
        }

        private ConfiguredActivity CreateConfiguredActivity(ConfiguredActivityModel model)
        {
            var activity = new ConfiguredActivity
            {
                ActivityName = model.ActivityName,
                Description = model.Description,
                Config = model.Config,
                Async = model.Async
            };

            if (activity.Async)
            {
                var delay = new TimeSpan(model.AsyncDelayDays, model.AsyncDelayHours, model.AsyncDelayMinutes, model.AsyncDelaySeconds);
                activity.AsyncDelay = (int)delay.TotalSeconds;
            }

            return activity;
        }

        public ActionResult AddActivity(string activityName)
        {
            var activity = _activityProvider.FindByName(activityName);

            ViewBag.Activity = activity;
            ViewBag.ConfigEditorVirtualPath = GetConfigEditorVirtualPath(activity);

            if (activity.ConfigModelType != null)
            {
                ViewBag.ConfigModel = TypeActivator.CreateInstance(activity.ConfigModelType);
            }

            return View();
        }

        public ActionResult EditActivity(string activityName)
        {
            var activity = _activityProvider.FindByName(activityName);

            ViewBag.Activity = activity;
            ViewBag.ConfigEditorVirtualPath = GetConfigEditorVirtualPath(activity);

            if (activity.ConfigModelType != null)
            {
                ViewBag.ConfigModel = TypeActivator.CreateInstance(activity.ConfigModelType);
            }

            return View();
        }

        static string GetConfigEditorVirtualPath(IActivity activity)
        {
            string configEditorVirtualPath = null;

            if (activity is IHasCustomActivityConfigEditor)
            {
                configEditorVirtualPath = ((IHasCustomActivityConfigEditor)activity).GetEditorVirtualPath(null, null);
            }
            else if (activity.ConfigModelType != null)
            {
                configEditorVirtualPath = "~/Areas/Commerce/Views/Rule/_DefaultActivityConfigEditor.cshtml";
            }
            return configEditorVirtualPath;
        }
    }
}
