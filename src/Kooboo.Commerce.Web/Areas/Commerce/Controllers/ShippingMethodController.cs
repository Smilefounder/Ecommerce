using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Commerce.Shipping.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Commerce.Web.Areas.Commerce.Models.ShippingMethods;
using Kooboo.Commerce.Shipping;
using Kooboo.Commerce.Web.Areas.Commerce.Models;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ShippingMethodController : CommerceControllerBase
    {
        private IShippingMethodService _shippingMethodService;
        private IShippingRateProviderFactory _shippingRateProviderFactory;
        private IShippingRateProviderViewsFactory _shippingRateProviderViewsFactory;

        public ShippingMethodController(
            IShippingMethodService shippingMethodService,
            IShippingRateProviderFactory shippingRateProviderFactory,
            IShippingRateProviderViewsFactory shippingRateProviderViewsFactory)
        {
            _shippingMethodService = shippingMethodService;
            _shippingRateProviderFactory = shippingRateProviderFactory;
            _shippingRateProviderViewsFactory = shippingRateProviderViewsFactory;
        }

        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            var methods = _shippingMethodService.Query()
                                               .OrderBy(x => x.Id)
                                               .ToPagedList(page, pageSize)
                                               .Transform(x => new ShippingMethodRowModel
                                               {
                                                   Id = x.Id,
                                                   Name = x.Name,
                                                   IsEnabled = x.IsEnabled,
                                                   ShippingRateProviderName = GetShippingRateProviderDisplayName(x.ShippingRateProviderName)
                                               });

            return View(methods);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Enable(ShippingMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var methodId = each.Id;
                var method = _shippingMethodService.GetById(methodId);
                _shippingMethodService.Enable(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, Transactional]
        public void EnableShippingMethod(int id)
        {
            var method = _shippingMethodService.GetById(id);
            _shippingMethodService.Enable(method);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Disable(ShippingMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var methodId = each.Id;
                var method = _shippingMethodService.GetById(methodId);
                _shippingMethodService.Disable(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Delete(ShippingMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var methodId = each.Id;
                var method = _shippingMethodService.GetById(methodId);
                _shippingMethodService.Delete(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Settings(ShippingMethodRowModel[] model)
        {
            var method = _shippingMethodService.GetById(model[0].Id);
            var views = _shippingRateProviderViewsFactory.FindByProviderName(method.ShippingRateProviderName);
            var url = Url.RouteUrl(views.Settings(method, ControllerContext), RouteValues.From(Request.QueryString).Merge("id", method.Id));

            return AjaxForm().RedirectTo(url);
        }

        [ChildActionOnly]
        public ActionResult Steps(int step)
        {
            ViewBag.Step = step;
            return PartialView();
        }

        public ActionResult BasicInfo(int? id)
        {
            var model = new ShippingMethodEditorModel
            {
                AvailableShippingRateProviders = _shippingRateProviderFactory.All().ToSelectList().ToList()
            };
            if (model.AvailableShippingRateProviders.Count > 0)
            {
                model.ShippingRateProviderName = model.AvailableShippingRateProviders[0].Value;
            }

            if (id != null)
            {
                var method = _shippingMethodService.GetById(id.Value);
                model.Id = method.Id;
                model.Name = method.Name;
                model.Description = method.Description;
                model.ShippingRateProviderName = method.ShippingRateProviderName;

                foreach (var field in method.CustomFields)
                {
                    model.CustomFields.Add(new NameValue
                    {
                        Name = field.Name,
                        Value = field.Value
                    });
                }
            }

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult BasicInfo(ShippingMethodEditorModel model)
        {
            var method = model.Id > 0 ? _shippingMethodService.GetById(model.Id) : new ShippingMethod();

            method.Name = model.Name;
            method.Description = model.Description;
            method.ShippingRateProviderName = model.ShippingRateProviderName;

            if (model.Id == 0)
            {
                _shippingMethodService.Create(method);
            }

            method.CustomFields.Clear();

            foreach (var field in model.CustomFields)
            {
                method.CustomFields.Add(new ShippingMethodCustomField
                {
                    ShippingMethodId = method.Id,
                    Name = field.Name,
                    Value = field.Value
                });
            }

            var views = _shippingRateProviderViewsFactory.FindByProviderName(method.ShippingRateProviderName);
            var url = Url.RouteUrl(views.Settings(method, ControllerContext), RouteValues.From(Request.QueryString).Merge("id", method.Id));

            return AjaxForm().RedirectTo(url);
        }

        public ActionResult Complete(int id)
        {
            var method = _shippingMethodService.GetById(id);
            return View(method);
        }

        private string GetShippingRateProviderDisplayName(string name)
        {
            var provider = _shippingRateProviderFactory.FindByName(name);
            return provider == null ? null : provider.DisplayName;
        }
    }
}