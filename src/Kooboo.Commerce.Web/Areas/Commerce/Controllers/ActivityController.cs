using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Activities;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Commerce.Activities.Services;
using Kooboo.Web.Mvc;
using System.Web.Routing;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Registry;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;

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

        public ActionResult Events(string category, int? page, int? pageSize)
        {
            ViewBag.Category = category;

            var eventTypes = _activityEventRegistry.GetEventTypesByCategory(category);
            var models = new List<ActivityEventRowModel>();

            foreach (var eventType in eventTypes)
            {
                var model = new ActivityEventRowModel
                {
                    EventType = eventType.GetVersionUnawareAssemblyQualifiedName(),
                    Name = eventType.GetDescription() ?? eventType.Name
                };

                models.Add(model);
            }

            return View(models.ToPagedList(page ?? 1, pageSize ?? 50));
        }

        [Transactional]
        public ActionResult List(string eventType)
        {
            var eventClrType = Type.GetType(eventType, true);

            ViewBag.CurrentEventType = eventClrType.GetVersionUnawareAssemblyQualifiedName();
            ViewBag.CurrentEventDisplayName = eventClrType.GetDescription() ?? eventClrType.Name;

            _activityRuleService.EnsureAlwaysRule(eventClrType);

            return View();
        }

        public ActionResult Create(int ruleId, string activityName)
        {
            var rule = _activityRuleService.GetById(ruleId);
            var activity = _activityFactory.FindByName(activityName);
            var model = new AttachedActivityModel
            {
                RuleId = rule.Id,
                ActivityName = activityName,
                ActivityDisplayName = activity.DisplayName
            };
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Create(AttachedActivityModel model)
        {
            var rule = _activityRuleService.GetById(model.RuleId);
            var attachedActivity = rule.AttacheActivity(model.Description, model.ActivityName, null);
            attachedActivity.IsEnabled = model.IsEnabled;

            CommerceContext.CurrentInstance.Database.SaveChanges();

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
            var attachedActivity = rule.FindAttachedActivity(attachedActivityId);
            var activity = _activityFactory.FindByName(attachedActivity.ActivityName);
            var views = _activityViewsFactory.FindByActivityName(attachedActivity.ActivityName);
            var eventType = Type.GetType(rule.EventType, true);

            var model = new AttachedActivityModel(attachedActivity);
            model.ActivityDisplayName = activity.DisplayName;

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Edit(AttachedActivityModel model)
        {
            var rule = _activityRuleService.GetById(model.RuleId);
            var attachedActivity = rule.FindAttachedActivity(model.Id);

            attachedActivity.Description = model.Description;
            attachedActivity.IsEnabled = model.IsEnabled;

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

        public ActionResult GetRules(string eventType)
        {
            var rules = _activityRuleService.GetRulesByEventType(Type.GetType(eventType, true)).ToList();
            var models = new List<ActivityRuleModel>();

            foreach (var rule in rules)
            {
                var model = new ActivityRuleModel(rule);
                foreach (var each in model.AttachedActivities)
                {
                    var views = _activityViewsFactory.FindByActivityName(each.ActivityName);
                    if (views != null)
                    {
                        each.ConfigUrl = Url.RouteUrl(views.Settings(rule.FindAttachedActivity(each.Id), ControllerContext), RouteValues.From(Request.QueryString));
                    }
                }

                models.Add(model);
            }

            return JsonNet(models).UseClientConvention();
        }

        [Transactional]
        public ActionResult CreateRule(string expression, string eventType)
        {
            var rule = _activityRuleService.Create(Type.GetType(eventType, true), expression);
            CommerceContext.CurrentInstance.Database.SaveChanges();

            return JsonNet(new ActivityRuleModel(rule)).UseClientConvention();
        }

        [Transactional]
        public ActionResult UpdateConditions(int ruleId, string expression)
        {
            var rule = _activityRuleService.GetById(ruleId);
            rule.ConditionsExpression = expression;
            return JsonNet(new
            {
                ConditionsExpression = expression,
                HighlightedConditionsExpression = new ConditionsExpressionHumanizer().Humanize(expression)
            }).UseClientConvention();
        }

        [Transactional]
        public void DeleteRule(int ruleId)
        {
            var rule = _activityRuleService.GetById(ruleId);
            _activityRuleService.Delete(rule);
        }

        public ActionResult GetAvailableActivities(string eventType)
        {
            var result = _activityFactory.FindBindableActivities(Type.GetType(eventType, true))
                                   .Select(x => new
                                   {
                                       Name = x.Name,
                                       DisplayName = x.DisplayName
                                   });

            return JsonNet(result).UseClientConvention();
        }

        public ActionResult GetAttachedActivity(int ruleId, int attachedActivityId)
        {
            var rule = _activityRuleService.GetById(ruleId);
            var attachedActivity = rule.FindAttachedActivity(attachedActivityId);
            return JsonNet(attachedActivity).UseClientConvention();
        }

        [HandleAjaxFormError, Transactional]
        public ActionResult DetachActivity(int ruleId, int attachedActivityId)
        {
            var rule = _activityRuleService.GetById(ruleId);
            rule.DetacheActivity(attachedActivityId);
            return AjaxForm();
        }
    }
}
