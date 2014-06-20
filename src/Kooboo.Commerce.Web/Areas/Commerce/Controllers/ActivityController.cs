using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Collections;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events.Registry;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Expressions.Formatting;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Activities;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Commerce.Web.Mvc.ModelBinding;
using Kooboo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ActivityController : CommerceControllerBase
    {
        private IEventRegistry _eventRegistry;
        private IActivityProvider _activityProvider;
        private IRepository<ActivityRule> _ruleRepository;

        public ActivityController(IEventRegistry eventRegistry, IActivityProvider activityProvider, IRepository<ActivityRule> ruleRepository)
        {
            _eventRegistry = eventRegistry;
            _activityProvider = activityProvider;
            _ruleRepository = ruleRepository;
        }

        public ActionResult Index()
        {
            var missingActivities = new HashSet<string>();
            var rules = _ruleRepository.Query().ToList();
            var categories = new List<EventCategoryModel>();

            foreach (var rule in rules)
            {
                if (rule.AttachedActivityInfos.Count == 0)
                {
                    continue;
                }

                var eventType = Type.GetType(rule.EventType, true);
                var entry = _eventRegistry.FindByType(eventType);
                var category = categories.Find(c => c.Name == entry.Category.Name);
                if (category == null)
                {
                    category = new EventCategoryModel
                    {
                        Name = entry.Category.Name,
                        Order = entry.Category.Order
                    };
                    categories.Add(category);
                }

                var events = category.Events.Find(e => e.EventType == entry.EventType);
                if (events == null)
                {
                    events = new EventRules
                    {
                        EventDisplayName = entry.DisplayName,
                        EventType = eventType,
                        Order = entry.Order
                    };
                    category.Events.Add(events);
                }

                events.Rules.Add(rule);

                // Check missing activities
                foreach (var name in rule.AttachedActivityInfos.Select(x => x.ActivityName).Distinct())
                {
                    if (!missingActivities.Contains(name))
                    {
                        var activity = _activityProvider.FindByName(name);
                        if (activity == null)
                        {
                            missingActivities.Add(name);
                        }
                    }
                }
            }

            foreach (var category in categories)
            {
                category.Events = category.Events.OrderBy(e => e.Order).ThenBy(e => e.EventDisplayName).ToList();
            }

            categories = categories.OrderBy(c => c.Order)
                                   .ThenBy(c => c.Name)
                                   .ToList();

            ViewBag.MissingActivities = missingActivities;

            return View(categories);
        }

        [Transactional]
        public ActionResult List(string eventType)
        {
            var eventClrType = Type.GetType(eventType, true);

            ViewBag.CurrentEventType = eventClrType.AssemblyQualifiedNameWithoutVersion();
            ViewBag.CurrentEventDisplayName = eventClrType.GetDescription() ?? eventClrType.Name.Humanize();

            _ruleRepository.EnsureAlwaysRule(eventClrType);

            return View();
        }

        public ActionResult CreateActivity(int ruleId, RuleBranch branch, string activityName)
        {
            var activity = _activityProvider.FindByName(activityName);
            var rule = _ruleRepository.Get(ruleId);

            var parametersEditorModel = new ActivityParametersEditorModel
            {
                RuleId = rule.Id
            };
            if (activity.ParametersType != null)
            {
                parametersEditorModel.Parameters = ActivityParameters.Create(activity.ParametersType);
            }

            ViewBag.Activity = activity;
            ViewBag.ParametersEditorModel = parametersEditorModel;

            return View(new ActivityEditorModel
            {
                RuleId = ruleId,
                RuleBranch = branch,
                Activity = new ActivityModel(activity, rule, null)
            });
        }

        public ActionResult EditActivity(int ruleId, int attachedActivityInfoId)
        {
            var rule = _ruleRepository.Get(ruleId);
            var attachedActivityInfo = rule.AttachedActivityInfos.Find(attachedActivityInfoId);
            var activity = _activityProvider.FindByName(attachedActivityInfo.ActivityName);

            var parametersEditorModel = new ActivityParametersEditorModel
            {
                RuleId = rule.Id,
                AttachedActivityInfoId = attachedActivityInfo.Id
            };
            if (activity.ParametersType != null)
            {
                parametersEditorModel.Parameters = ActivityParameters.Create(activity.ParametersType, attachedActivityInfo.GetParameters());
            }

            ViewBag.Activity = activity;
            ViewBag.ParametersEditorModel = parametersEditorModel;

            return View(new ActivityEditorModel
            {
                RuleId = ruleId,
                AttachedActivityInfoId = attachedActivityInfoId,
                Activity = new ActivityModel(activity, rule, attachedActivityInfo)
            });
        }

        public ActionResult GetActivityEditorModel(int ruleId, RuleBranch branch, string activityName, int attachedActivityInfoId)
        {
            var model = new ActivityEditorModel();
            var rule = _ruleRepository.Get(ruleId);

            model.RuleId = rule.Id;
            model.RuleBranch = branch;

            var activity = _activityProvider.FindByName(activityName);
            AttachedActivityInfo attachedActivityInfo = null;
            if (attachedActivityInfoId > 0)
            {
                attachedActivityInfo = rule.AttachedActivityInfos.Find(attachedActivityInfoId);
            }

            model.Activity = new ActivityModel(activity, rule, attachedActivityInfo);

            if (attachedActivityInfoId > 0)
            {
                model.AttachedActivityInfoId = attachedActivityInfo.Id;
                model.Description = attachedActivityInfo.Description;
                model.Priority = attachedActivityInfo.Priority;
                model.IsAsyncExecutionEnabled = attachedActivityInfo.IsAsyncExeuctionEnabled;

                var delay = TimeSpan.FromSeconds(attachedActivityInfo.AsyncExecutionDelay);
                model.DelayDays = delay.Days;
                model.DelayHours = delay.Hours;
                model.DelayMinutes = delay.Minutes;
                model.DelaySeconds = delay.Seconds;
            }

            return JsonNet(model).UsingClientConvention();
        }

        [HttpPost, HandleAjaxError, Transactional]
        public ActionResult SaveActivity(ActivityEditorModel model)
        {
            var rule = _ruleRepository.Get(model.RuleId);
            AttachedActivityInfo activityInfo = null;

            if (model.AttachedActivityInfoId > 0)
            {
                activityInfo = rule.AttachedActivityInfos.Find(model.AttachedActivityInfoId);
            }
            else
            {
                activityInfo = rule.AttachActivity(model.RuleBranch, model.Description, model.Activity.Name);
            }

            activityInfo.Description = model.Description;
            activityInfo.IsEnabled = model.IsEnabled;

            if (model.IsAsyncExecutionEnabled)
            {
                var delay = new TimeSpan(model.DelayDays, model.DelayHours, model.DelayMinutes, model.DelaySeconds);

                if (activityInfo.IsAsyncExeuctionEnabled)
                {
                    activityInfo.UpdateAsyncExecutionDelay((int)delay.TotalSeconds);
                }
                else
                {
                    activityInfo.EnableAsyncExecution((int)delay.TotalSeconds);
                }
            }
            else
            {
                activityInfo.DisableAsyncExecution();
            }

            CommerceContext.CurrentInstance.Database.SaveChanges();

            return JsonNet(new
            {
                RuleId = rule.Id,
                AttachedActivityInfoId = activityInfo.Id
            })
            .UsingClientConvention();
        }

        [HttpPost, HandleAjaxError, Transactional]
        public void UpdateActivityParameters(int ruleId, int attachedActivityInfoId, [ModelBinder(typeof(DerivedTypeModelBinder))]ActivityParameters parameters)
        {
            var rule = _ruleRepository.Get(ruleId);
            var attachedActivityInfo = rule.AttachedActivityInfos.Find(attachedActivityInfoId);
            attachedActivityInfo.SetParameters(parameters == null ? null : parameters.GetValues());
        }

        [HandleAjaxError]
        public ActionResult GetRules(string eventType)
        {
            var rules = _ruleRepository.Query()
                                       .ByEvent(Type.GetType(eventType, true))
                                       .OrderBy(x => x.Type)
                                       .ThenBy(x => x.Id)
                                       .ToList();

            var models = new List<ActivityRuleModel>();

            foreach (var rule in rules)
            {
                var model = new ActivityRuleModel(rule);
                models.Add(model);
            }

            return JsonNet(models).UsingClientConvention();
        }

        [Transactional]
        public ActionResult CreateRule(CreateRuleModel model)
        {
            var rule = new ActivityRule(Type.GetType(model.EventType, true), RuleType.Normal);
            rule.Conditions = model.Conditions;
            _ruleRepository.Insert(rule);
            return JsonNet(new ActivityRuleModel(rule)).UsingClientConvention();
        }

        [Transactional]
        public ActionResult UpdateConditions(UpdateConditionsModel model)
        {
            var rule = _ruleRepository.Get(model.RuleId);
            rule.Conditions = model.Conditions;
            return JsonNet(new ActivityRuleModel(rule)).UsingClientConvention();
        }

        [Transactional]
        public void DeleteRule(int ruleId)
        {
            var rule = _ruleRepository.Get(ruleId);
            _ruleRepository.Delete(rule);
        }

        public ActionResult GetAvailableActivities(string eventType)
        {
            var result = _activityProvider.FindBindableTo(Type.GetType(eventType, true))
                                          .Select(x => new
                                          {
                                              Name = x.Name,
                                              DisplayName = x.DisplayName
                                          });

            return JsonNet(result).UsingClientConvention();
        }

        public ActionResult GetAttachedActivityInfo(int ruleId, int attachedActivityInfoId)
        {
            var rule = _ruleRepository.Get(ruleId);
            var attachedActivity = rule.AttachedActivityInfos.Find(attachedActivityInfoId);
            return JsonNet(new AttachedActivityModel(attachedActivity)).UsingClientConvention();
        }

        [HandleAjaxFormError, Transactional]
        public ActionResult DetachActivity(int ruleId, int attachedActivityInfoId)
        {
            var rule = _ruleRepository.Get(ruleId);
            rule.DetachActivity(attachedActivityInfoId);
            return AjaxForm();
        }
    }
}
