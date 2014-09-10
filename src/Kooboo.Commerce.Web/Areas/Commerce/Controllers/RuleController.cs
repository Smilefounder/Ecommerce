using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;
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
using Kooboo.Commerce.Web.Framework.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class RuleController : CommerceController
    {
        private IActivityProvider _activityProvider;

        public RuleController(IActivityProvider activityProvider)
        {
            _activityProvider = activityProvider;
        }

        public ActionResult Index()
        {
            var slotManager = EventSlotManager.Instance;
            var ruleManager = RuleManager.GetManager(CurrentInstance.Name);
            var models = new List<EventSlotModel>();

            foreach (var group in slotManager.GetGroups())
            {
                var slots = slotManager.GetSlots(group);
                foreach (var slot in slots)
                {
                    var rules = ruleManager.GetRules(slot.EventType.Name);
                    if (rules.Any())
                    {
                        var model = new EventSlotModel
                        {
                            EventName = slot.EventType.Name
                        };

                        foreach (var rule in rules)
                        {
                            model.Rules.Add(RuleModelBase.FromRule(rule));
                        }

                        models.Add(model);
                    }
                }
            }

            return View(models);
        }

        public ActionResult List(string eventName)
        {
            var slot = EventSlotManager.Instance.GetSlot(eventName);
            var availableActivities = _activityProvider.FindBindableTo(slot.EventType)
                                                       .Select(x => new
                                                       {
                                                           x.Name,
                                                           x.DisplayName
                                                       })
                                                       .ToList();

            ViewBag.EventName = slot.EventType.Name;
            ViewBag.EventTypeName = slot.EventType.AssemblyQualifiedNameWithoutVersion();
            ViewBag.AvailableActivities = availableActivities;
            ViewBag.AvailableParameters = RuleParameterProviders.Providers.GetParameters(slot.EventType).ToList();

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
            var slot = EventSlotManager.Instance.GetSlot(eventName);
            var models = JsonConvert.DeserializeObject<List<RuleModelBase>>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });         
            var rules = new List<Rule>();

            foreach (var model in models)
            {
                rules.Add(model.ToRule(slot));
            }

            var manager = RuleManager.GetManager(CurrentInstance.Name);
            manager.SaveRules(eventName, rules);
        }

        public ActionResult AddActivity(string activityName)
        {
            var activity = _activityProvider.FindByName(activityName);

            ViewBag.Activity = activity;
            ViewBag.ConfigEditorVirtualPath = GetConfigEditorVirtualPath(activity);

            if (activity.ConfigType != null)
            {
                ViewBag.ConfigModel = TypeActivator.CreateInstance(activity.ConfigType);
            }

            return View();
        }

        public ActionResult EditActivity(string activityName)
        {
            var activity = _activityProvider.FindByName(activityName);

            ViewBag.Activity = activity;
            ViewBag.ConfigEditorVirtualPath = GetConfigEditorVirtualPath(activity);

            if (activity.ConfigType != null)
            {
                ViewBag.ConfigModel = TypeActivator.CreateInstance(activity.ConfigType);
            }

            return View();
        }

        static string GetConfigEditorVirtualPath(IActivity activity)
        {
            string configEditorVirtualPath = null;

            if (activity is IHasCustomActivityConfigEditor)
            {
                configEditorVirtualPath = ((IHasCustomActivityConfigEditor)activity).GetEditorVirtualPath();
            }
            else if (activity.ConfigType != null)
            {
                configEditorVirtualPath = "~/Areas/Commerce/Views/Rule/_DefaultActivityConfigEditor.cshtml";
            }
            return configEditorVirtualPath;
        }
    }
}
