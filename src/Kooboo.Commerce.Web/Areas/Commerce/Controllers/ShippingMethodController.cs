using Kooboo.CMS.Common.Runtime.Dependency;
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
            var url = Url.RouteUrl(views.Settings(method, ControllerContext), RouteValues.From(Request.QueryString));

            return AjaxForm().RedirectTo(url);
        }

        public ActionResult Create()
        {
            var model = new ShippingMethodEditorModel
            {
                AvailableShippingRateProviders = _shippingRateProviderFactory.All().ToSelectList().ToList()
            };

            if (model.AvailableShippingRateProviders.Count > 0)
            {
                model.ShippingRateProviderName = model.AvailableShippingRateProviders[0].Value;
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var method = _shippingMethodService.GetById(id);
            var model = new ShippingMethodEditorModel
            {
                Id = method.Id,
                Name = method.Name,
                Description = method.Description,
                IsEnabled = method.IsEnabled,
                ShippingRateProviderName = method.ShippingRateProviderName,
                AvailableShippingRateProviders = _shippingRateProviderFactory.All().ToSelectList().ToList()
            };

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(ShippingMethodEditorModel model)
        {
            var method = model.Id > 0 ? _shippingMethodService.GetById(model.Id) : new ShippingMethod();

            method.Name = model.Name;
            method.Description = model.Description;
            method.ShippingRateProviderName = model.ShippingRateProviderName;

            if (model.Id == 0)
            {
                _shippingMethodService.Create(method);
            }
            else
            {
                _shippingMethodService.Update(method);
            }

            if (model.IsEnabled)
            {
                _shippingMethodService.Enable(method);
            }
            else
            {
                _shippingMethodService.Disable(method);
            }

            var views = _shippingRateProviderViewsFactory.FindByProviderName(method.ShippingRateProviderName);
            var url = Url.RouteUrl(views.Settings(method, ControllerContext), RouteValues.From(Request.QueryString));

            return AjaxForm().RedirectTo(url);
        }

        private string GetShippingRateProviderDisplayName(string name)
        {
            var provider = _shippingRateProviderFactory.FindByName(name);
            return provider == null ? null : provider.DisplayName;
        }
    }
}