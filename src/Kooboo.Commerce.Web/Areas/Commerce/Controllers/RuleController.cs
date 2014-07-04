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
using Kooboo.Commerce.Rules.Parameters;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions;

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
            ViewBag.AvailableParameters = ParameterProviders.Providers.GetParameters(eventEntry.EventType).ToList();

            var manager = RuleManager.GetManager(CurrentInstance.Name);
            var rules = manager.GetRules(eventName);

            var models = new List<RuleModelBase>();
            foreach (var rule in rules)
            {
                models.Add(RuleModelBase.FromRule(rule));
            }

            return View(models);
        }

        [HttpPost, HandleAjaxError]
        public void Save(string eventName, string json)
        {
            var @event = ActivityEventManager.Instance.FindEvent(eventName);
            var models = JsonConvert.DeserializeObject<List<RuleModelBase>>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });         
            var rules = new List<RuleBase>();

            foreach (var model in models)
            {
                rules.Add(model.ToRule(@event));
            }

            var manager = RuleManager.GetManager(CurrentInstance.Name);
            manager.SaveRules(eventName, rules);
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
