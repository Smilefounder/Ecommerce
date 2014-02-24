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

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ActivityController : CommerceControllerBase
    {
        private IEventRegistry _eventRegistry;
        private IActivityBindingService _bindingService;
        private IActivityFactory _activityFactory;
        private IActivityViewsFactory _activityViewsFactory;

        public ActivityController(
            IEventRegistry eventRegistry,
            IActivityBindingService bindingService,
            IActivityFactory activityFactory,
            IActivityViewsFactory activityViewsFactory)
        {
            _eventRegistry = eventRegistry;
            _bindingService = bindingService;
            _activityFactory = activityFactory;
            _activityViewsFactory = activityViewsFactory;
        }

        public ActionResult Events(string category, int? page, int? pageSize)
        {
            ViewBag.Category = category;

            var eventTypes = _eventRegistry.FindEventsByCategory(category);
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

        public ActionResult Create(string eventType, string activityName)
        {
            var eventClrType = Type.GetType(eventType, true);
            var views = _activityViewsFactory.FindByActivityName(activityName);
            var activity = _activityFactory.FindByName(activityName);
            var model = new ActivityBindingEditorModel
            {
                EventClrType = eventType,
                EventDisplayName = eventClrType.GetDescription() ?? eventClrType.Name,
                ActivityName = activityName,
                ActivityDisplayName = activity.DisplayName,
                IsConfigurable = views != null
            };
            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Create(ActivityBindingEditorModel model, string @return)
        {
            var eventType = Type.GetType(model.EventClrType, true);

            var binding = ActivityBinding.Create(eventType, model.ActivityName, model.Description);
            _bindingService.Create(binding);

            if (model.IsEnabled)
            {
                _bindingService.Enable(binding);
            }
            else
            {
                _bindingService.Disable(binding);
            }

            CommerceContext.CurrentInstance.Database.SaveChanges();

            if (model.IsConfigurable)
            {
                var views = _activityViewsFactory.FindByActivityName(binding.ActivityName);
                var url = Url.RouteUrl(views.Settings(binding, ControllerContext), RouteValues.From(Request.QueryString));

                return AjaxForm().RedirectTo(url);
            }

            return AjaxForm().RedirectTo(@return);
        }

        public ActionResult Edit(int id)
        {
            var binding = _bindingService.GetById(id);
            var activity = _activityFactory.FindByName(binding.ActivityName);
            var views = _activityViewsFactory.FindByActivityName(binding.ActivityName);
            var eventType = Type.GetType(binding.EventClrType, true);

            var model = new ActivityBindingEditorModel
            {
                Id = id,
                Description = binding.Description,
                ActivityName = binding.ActivityName,
                EventClrType = binding.EventClrType,
                EventDisplayName = eventType.GetDescription() ?? eventType.Name,
                ActivityDisplayName = activity.DisplayName,
                IsConfigurable = views != null,
                IsEnabled = binding.IsEnabled,
                Priority = binding.Priority
            };

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Edit(ActivityBindingEditorModel model, string @return)
        {
            var binding = _bindingService.GetById(model.Id);
            binding.Description = model.Description;

            if (model.IsEnabled)
            {
                _bindingService.Enable(binding);
            }
            else
            {
                _bindingService.Disable(binding);
            }

            _bindingService.Update(binding);

            if (model.IsConfigurable)
            {
                var views = _activityViewsFactory.FindByActivityName(binding.ActivityName);
                var url = Url.RouteUrl(views.Settings(binding, ControllerContext), RouteValues.From(Request.QueryString));

                return AjaxForm().RedirectTo(url);
            }

            return AjaxForm().RedirectTo(@return);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Settings(ActivityBindingRowModel[] model)
        {
            var binding = _bindingService.GetById(model[0].Id);
            var views = _activityViewsFactory.FindByActivityName(binding.ActivityName);
            var url = Url.RouteUrl(views.Settings(binding, ControllerContext), RouteValues.From(Request.QueryString));

            return AjaxForm().RedirectTo(url);
        }

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
    }
}
