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
        private IActivityBindingService _bindingService;
        private IActivityRuleService _activityRuleService;
        private IActivityFactory _activityFactory;
        private IActivityViewsFactory _activityViewsFactory;

        public ActivityController(
            IActivityEventRegistry activityEventRegistry,
            IActivityBindingService bindingService,
            IActivityRuleService activityRuleService,
            IActivityFactory activityFactory,
            IActivityViewsFactory activityViewsFactory)
        {
            _activityEventRegistry = activityEventRegistry;
            _bindingService = bindingService;
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

        public ActionResult List(string eventType, int? page, int? pageSize)
        {
            var eventClrType = Type.GetType(eventType, true);
            var bindableActivities = _activityFactory.FindBindableActivities(eventClrType);

            ViewBag.AllActivities = bindableActivities.Select(x => new SelectListItemEx
            {
                Text = x.DisplayName,
                Value = x.Name
            })
            .ToList();

            ViewBag.CurrentEventType = eventClrType.GetVersionUnawareAssemblyQualifiedName();
            ViewBag.CurrentEventDisplayName = eventClrType.GetDescription() ?? eventClrType.Name;


            var bindings = _bindingService.Query()
                                         .WhereBoundToEvent(eventClrType)
                                         .OrderByDescending(x => x.Priority)
                                         .ThenBy(x => x.Id)
                                         .ToPagedList(page ?? 1, pageSize ?? 50)
                                         .Transform(binding =>
                                         {
                                             var views = _activityViewsFactory.FindByActivityName(binding.ActivityName);
                                             var model = new ActivityBindingRowModel
                                             {
                                                 Id = binding.Id,
                                                 Description = binding.Description,
                                                 Configurable = views != null,
                                                 IsEnabled = binding.IsEnabled,
                                                 Priority = binding.Priority
                                             };

                                             return model;
                                         });

            return View(bindings);
        }

        public ActionResult Create(int ruleId, string activityName)
        {
            var rule = _activityRuleService.GetById(ruleId);
            var eventClrType = Type.GetType(rule.EventType, true);
            var views = _activityViewsFactory.FindByActivityName(activityName);
            var activity = _activityFactory.FindByName(activityName);
            var model = new ActivityBindingEditorModel
            {
                RuleId = rule.Id,
                EventClrType = rule.EventType,
                EventDisplayName = eventClrType.GetDescription() ?? eventClrType.Name,
                ActivityName = activityName,
                ActivityDisplayName = activity.DisplayName,
                IsConfigurable = views != null
            };
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Create(ActivityBindingEditorModel model)
        {
            var rule = _activityRuleService.GetById(model.RuleId);
            var attachedActivity = rule.AttacheActivity(model.Description, model.ActivityName, null);
            attachedActivity.IsEnabled = model.IsEnabled;

            CommerceContext.CurrentInstance.Database.SaveChanges();

            var configUrl = String.Empty;

            if (model.IsConfigurable)
            {
                var views = _activityViewsFactory.FindByActivityName(attachedActivity.ActivityName);
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

            var model = new ActivityBindingEditorModel
            {
                Id = attachedActivityId,
                RuleId = rule.Id,
                Description = attachedActivity.Description,
                ActivityName = attachedActivity.ActivityName,
                EventClrType = rule.EventType,
                EventDisplayName = eventType.GetDescription() ?? eventType.Name,
                ActivityDisplayName = activity.DisplayName,
                IsConfigurable = views != null,
                IsEnabled = attachedActivity.IsEnabled,
                Priority = attachedActivity.Priority
            };

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Edit(ActivityBindingEditorModel model)
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

        //[HttpPost, HandleAjaxFormError]
        //public ActionResult Settings(ActivityBindingRowModel[] model)
        //{
        //    var binding = _bindingService.GetById(model[0].Id);
        //    var views = _activityViewsFactory.FindByActivityName(binding.ActivityName);
        //    var url = Url.RouteUrl(views.Settings(binding, ControllerContext), RouteValues.From(Request.QueryString));

        //    return AjaxForm().RedirectTo(url);
        //}

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Enable(ActivityBindingRowModel[] model)
        {
            foreach (var item in model)
            {
                var binding = _bindingService.GetById(item.Id);
                _bindingService.Enable(binding);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Disable(ActivityBindingRowModel[] model)
        {
            foreach (var item in model)
            {
                var binding = _bindingService.GetById(item.Id);
                _bindingService.Disable(binding);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Delete(ActivityBindingRowModel[] model, string @return)
        {
            foreach (var item in model)
            {
                var binding = _bindingService.GetById(item.Id);
                _bindingService.Delete(binding);
            }

            return AjaxForm().ReloadPage();
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

            return JsonNet(models).Camelcased();
        }

        [Transactional]
        public ActionResult CreateRule(string expression, string eventType)
        {
            var rule = _activityRuleService.Create(Type.GetType(eventType, true), expression);
            CommerceContext.CurrentInstance.Database.SaveChanges();

            return JsonNet(new ActivityRuleModel(rule)).Camelcased();
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
            }).Camelcased();
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

            return JsonNet(result).Camelcased();
        }

        public ActionResult GetAttachedActivity(int ruleId, int attachedActivityId)
        {
            var rule = _activityRuleService.GetById(ruleId);
            var attachedActivity = rule.FindAttachedActivity(attachedActivityId);
            return JsonNet(attachedActivity).Camelcased();
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
