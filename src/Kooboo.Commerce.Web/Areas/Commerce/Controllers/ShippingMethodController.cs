using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Commerce.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Commerce.Web.Areas.Commerce.Models.ShippingMethods;
using Kooboo.Commerce.Web.Areas.Commerce.Models;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ShippingMethodController : CommerceController
    {
        private ShippingMethodService _shippingMethodService;
        private IShippingRateProviderFactory _shippingRateProviderFactory;

        public ShippingMethodController(
            ShippingMethodService shippingMethodService,
            IShippingRateProviderFactory shippingRateProviderFactory)
        {
            _shippingMethodService = shippingMethodService;
            _shippingRateProviderFactory = shippingRateProviderFactory;
        }

        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            var methods = _shippingMethodService.Query()
                                                .OrderBy(x => x.Id)
                                                .Paginate(page - 1, pageSize)
                                                .Transform(x => new ShippingMethodRowModel
                                                {
                                                    Id = x.Id,
                                                    Name = x.Name,
                                                    IsEnabled = x.IsEnabled,
                                                    ShippingRateProviderName = x.ShippingRateProviderName
                                                })
                                                .ToPagedList();

            return View(methods);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Enable(ShippingMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var methodId = each.Id;
                var method = _shippingMethodService.Find(methodId);
                _shippingMethodService.Enable(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, Transactional]
        public void EnableShippingMethod(int id)
        {
            var method = _shippingMethodService.Find(id);
            _shippingMethodService.Enable(method);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Disable(ShippingMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var methodId = each.Id;
                var method = _shippingMethodService.Find(methodId);
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
                var method = _shippingMethodService.Find(methodId);
                _shippingMethodService.Delete(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Settings(ShippingMethodRowModel[] model)
        {
            return AjaxForm().RedirectTo(Url.Action("ShippingRateProvider", RouteValues.From(Request.QueryString).Merge("id", model[0].Id)));
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
                var method = _shippingMethodService.Find(id.Value);
                model.Id = method.Id;
                model.Name = method.Name;
                model.Description = method.Description;
                model.ShippingRateProviderName = method.ShippingRateProviderName;
            }

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult BasicInfo(ShippingMethodEditorModel model)
        {
            var method = model.Id > 0 ? _shippingMethodService.Find(model.Id) : new ShippingMethod();

            method.Name = model.Name;
            method.Description = model.Description;
            method.ShippingRateProviderName = model.ShippingRateProviderName;

            if (model.Id == 0)
            {
                _shippingMethodService.Create(method);
            }

            var shippingRateProvider = _shippingRateProviderFactory.FindByName(method.ShippingRateProviderName);
            string redirectUrl = null;

            var editor = shippingRateProvider as IHasCustomShippingRateProviderConfigEditor;
            if (editor != null || shippingRateProvider.ConfigType != null)
            {
                redirectUrl = Url.Action("ShippingRateProvider", RouteValues.From(Request.QueryString).Merge("id", method.Id));
            }
            else
            {
                redirectUrl = Url.Action("Complete", RouteValues.From(Request.QueryString).Merge("id", method.Id));
            }

            return AjaxForm().RedirectTo(redirectUrl);
        }

        public ActionResult ShippingRateProvider(int id)
        {
            var method = _shippingMethodService.Find(id);
            var shippingRateProvider = _shippingRateProviderFactory.FindByName(method.ShippingRateProviderName);
            var editorModel = new ShippingRateProviderConfigEditorModel
            {
                ShippingMethodId = method.Id,
                Config = method.LoadShippingRateProviderConfig(shippingRateProvider.ConfigType) ?? Activator.CreateInstance(shippingRateProvider.ConfigType)
            };

            ViewBag.ShippingRateProvider = shippingRateProvider;
            ViewBag.ShippingRateProviderConfigEditorModel = editorModel;

            return View(method);
        }

        [HttpPost, HandleAjaxError, Transactional]
        public void UpdateShippingRateProviderConfig(int shippingMethodId, [ModelBinder(typeof(BindingTypeAwareModelBinder))]object config)
        {
            var method = _shippingMethodService.Find(shippingMethodId);
            method.UpdateShippingRateProviderConfig(config);
        }

        public ActionResult Complete(int id)
        {
            var method = _shippingMethodService.Find(id);
            return View(method);
        }
    }
}