using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Activities;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ActivityController : CommerceControllerBase
    {
        private IActivityEventRegistry _activityEventRegistry;
        private IActivityRuleService _activityRuleService;
        private IActivityFactory _activityFactory;
        private IActivityViewsFactory _activityViewsFactory;

        public ActivityController(
            IActivityEventRegistry activityEventRegistry,
            IActivityRuleService activityRuleService,
            IActivityFactory activityFactory,
            IActivityViewsFactory activityViewsFactory)
        {
            _activityEventRegistry = activityEventRegistry;
            _activityRuleService = activityRuleService;
            _activityFactory = activityFactory;
            _activityViewsFactory = activityViewsFactory;
        }

        [Transactional]
        public ActionResult List(string eventType)
        {
            var eventClrType = Type.GetType(eventType, true);

            ViewBag.CurrentEventType = eventClrType.AssemblyQualifiedNameWithoutVersion();
            ViewBag.CurrentEventDisplayName = eventClrType.GetDescription() ?? eventClrType.Name.Humanize();

            _activityRuleService.EnsureAlwaysRule(eventClrType);

            return View();
        }

        public ActionResult Create(int ruleId, RuleBranch branch, string activityName)
        {
            var rule = _activityRuleService.GetById(ruleId);
            var activity = _activityFactory.FindByName(activityName);
            var model = new AttachedActivityModel
            {
                RuleId = rule.Id,
                RuleBranch = branch,
                ActivityName = activityName,
                ActivityDisplayName = activity.DisplayName,
                ActivityAllowAsyncExecution = activity.AllowAsyncExecution
            };
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(AttachedActivityModel model)
        {
            var rule = _activityRuleService.GetById(model.RuleId);
            AttachedActivity attachedActivity = null;

            if (model.Id > 0)
            {
                attachedActivity = rule.AttachedActivities.ById(model.Id);
            }
            else
            {
                attachedActivity = rule.AttachActivity(model.RuleBranch, model.Description, model.ActivityName, null);
            }

            attachedActivity.Description = model.Description;
            attachedActivity.IsEnabled = model.IsEnabled;

            if (model.EnableAsyncExecution)
            {
                var delay = new TimeSpan(model.DelayDays, model.DelayHours, model.DelayMinutes, model.DelaySeconds);

                if (attachedActivity.IsAsyncExeuctionEnabled)
                {
                    attachedActivity.UpdateAsyncExecutionDelay((int)delay.TotalSeconds);
                }
                else
                {
                    attachedActivity.EnableAsyncExecution((int)delay.TotalSeconds);
                }
            }
            else
            {
                attachedActivity.DisableAsyncExecution();
            }

            CommerceContext.CurrentInstance.Database.Commit();

            var configUrl = String.Empty;

            var views = _activityViewsFactory.FindByActivityName(attachedActivity.ActivityName);
            if (views != null)
            {
                configUrl = Url.RouteUrl(views.Settings(attachedActivity, ControllerContext), RouteValues.From(Request.QueryString));
            }

            return AjaxForm().WithModel(new
            {
                RuleId = rule.Id,
                AttachedActivityId = attachedActivity.Id,
                ConfigUrl = configUrl
            });
        }

        public ActionResult Edit(int ruleId, int attachedActivityId)
        {
            var rule = _activityRuleService.GetById(ruleId);
            var attachedActivity = rule.AttachedActivities.ById(attachedActivityId);
            var activity = _activityFactory.FindByName(attachedActivity.ActivityName);
            var views = _activityViewsFactory.FindByActivityName(attachedActivity.ActivityName);
            var eventType = Type.GetType(rule.EventType, true);

            var model = new AttachedActivityModel(attachedActivity);
            model.ActivityDisplayName = activity.DisplayName;
            model.ActivityAllowAsyncExecution = activity.AllowAsyncExecution;

            if (!model.ActivityAllowAsyncExecution)
            {
                model.EnableAsyncExecution = false;
            }

            return View(model);
        }

        [HandleAjaxError]
        public ActionResult GetRules(string eventType)
        {
            var rules = _activityRuleService.GetRulesByEventType(Type.GetType(eventType, true)).ToList();
            var models = new List<ActivityRuleModel>();

            foreach (var rule in rules)
            {
                var model = new ActivityRuleModel(rule);
                model.ForEachAttachedActivity(x =>
                {
                    x.ConfigUrl = GetActivityConfigUrl(rule, rule.AttachedActivities.ById(x.Id));
                });

                models.Add(model);
            }

            return JsonNet(models).UsingClientConvention();
        }

        [Transactional]
        public ActionResult CreateRule(string expression, string eventType)
        {
            var rule = _activityRuleService.Create(Type.GetType(eventType, true), expression);
            return JsonNet(new ActivityRuleModel(rule)).UsingClientConvention();
        }

        [Transactional]
        public ActionResult UpdateConditions(int ruleId, string expression)
        {
            var rule = _activityRuleService.GetById(ruleId);
            rule.ConditionsExpression = expression;

            return JsonNet(new
            {
                ConditionsExpression = expression,
                HighlightedConditionsExpression = new ConditionsExpressionPrettifier().Prettify(expression, Type.GetType(rule.EventType, true))
            }).UsingClientConvention();
        }

        [Transactional]
        public void DeleteRule(int ruleId)
        {
            var rule = _activityRuleService.GetById(ruleId);
            _activityRuleService.Delete(rule);
        }

        public ActionResult GetAvailableActivities(string eventType)
        {
            var result = _activityFactory.FindActivitiesBindableTo(Type.GetType(eventType, true))
                                   .Select(x => new
                                   {
                                       Name = x.Name,
                                       DisplayName = x.DisplayName
                                   });

            return JsonNet(result).UsingClientConvention();
        }

        public ActionResult GetAttachedActivity(int ruleId, int attachedActivityId)
        {
            var rule = _activityRuleService.GetById(ruleId);
            var attachedActivity = rule.AttachedActivities.ById(attachedActivityId);
            return JsonNet(new AttachedActivityModel(attachedActivity)
            {
                ConfigUrl = GetActivityConfigUrl(rule, attachedActivity)
            }).UsingClientConvention();
        }

        private string GetActivityConfigUrl(ActivityRule rule, AttachedActivity attachedActivity)
        {
            var views = _activityViewsFactory.FindByActivityName(attachedActivity.ActivityName);
            if (views != null)
            {
                return Url.RouteUrl(views.Settings(rule.AttachedActivities.ById(attachedActivity.Id), ControllerContext), RouteValues.From(Request.QueryString));
            }

            return null;
        }

        [HandleAjaxFormError, Transactional]
        public ActionResult DetachActivity(int ruleId, int attachedActivityId)
        {
            var rule = _activityRuleService.GetById(ruleId);
            rule.DetachActivity(attachedActivityId);
            return AjaxForm();
        }
    }
}
