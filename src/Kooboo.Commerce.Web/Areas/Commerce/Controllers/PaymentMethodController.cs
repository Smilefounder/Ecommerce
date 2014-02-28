using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Web.Areas.Commerce.Models.PaymentMethods;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Commerce.Payments.Services;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class PaymentMethodController : CommerceControllerBase
    {
        private IPaymentProcessorFactory _gatewayFactory;
        private IPaymentProcessorViewsFactory _gatewayViewsFactory;
        private IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(
            IPaymentProcessorFactory gatewayFactory,
            IPaymentProcessorViewsFactory gatewayViewFactory,
            IPaymentMethodService paymentMethodService)
        {
            _gatewayFactory = gatewayFactory;
            _gatewayViewsFactory = gatewayViewFactory;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            var methods = _paymentMethodService.Query()
                                 .OrderByDescending(x => x.Id)
                                 .ToPagedList(page ?? 1, pageSize ?? 50)
                                 .Transform(x =>
                                 {
                                     var model = new PaymentMethodRowModel(x);
                                     var views = _gatewayViewsFactory.FindByPaymentProcessor(model.PaymentGatewayName);
                                     model.IsConfigurable = views != null;

                                     return model;
                                 });
            return View(methods);
        }

        public ActionResult Create()
        {
            var model = new PaymentMethodEditorModel();
            model.AvailableGateways = GetAvailableGateways();

            if (model.AvailableGateways.Count > 0)
            {
                model.DisplayName = model.AvailableGateways[0].Text;
                model.PaymentGateway = model.AvailableGateways[0].Text;
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var method = _paymentMethodService.GetById(id);
            var model = new PaymentMethodEditorModel
            {
                Id = method.Id,
                DisplayName = method.DisplayName,
                PaymentGateway = method.PaymentProcessor,
                AdditionalFeeChargeMode = method.AdditionalFeeChargeMode,
                AdditionalFeeAmount = method.AdditionalFeeAmount,
                AdditionalFeePercent = method.AdditionalFeePercent,
                IsEnabled = method.IsEnabled,
                IsEdit = true
            };

            model.AvailableGateways = GetAvailableGateways();

            return View(model);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Settings(PaymentMethodRowModel[] model)
        {
            var method = _paymentMethodService.GetById(model[0].Id);
            var views = _gatewayViewsFactory.FindByPaymentProcessor(method.PaymentProcessor);
            var url = Url.RouteUrl(views.Settings(method, ControllerContext), RouteValues.From(Request.QueryString));

            return AjaxForm().RedirectTo(url);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Enable(PaymentMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var method = _paymentMethodService.GetById(each.Id);
                _paymentMethodService.Enable(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Disable(PaymentMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var method = _paymentMethodService.GetById(each.Id);
                _paymentMethodService.Disable(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Delete(PaymentMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var method = _paymentMethodService.GetById(each.Id);
                _paymentMethodService.Delete(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Save(PaymentMethodEditorModel model, string @return)
        {
            PaymentMethod method = null;

            if (model.Id > 0)
            {
                method = _paymentMethodService.GetById(model.Id);
                model.UpdateTo(method);
            }
            else
            {
                method = new PaymentMethod();
                model.UpdateTo(method);
                _paymentMethodService.Create(method);
            }

            if (model.IsEnabled)
            {
                _paymentMethodService.Enable(method);
            }
            else
            {
                _paymentMethodService.Disable(method);
            }

            CommerceContext.CurrentInstance.Database.SaveChanges();

            var views = _gatewayViewsFactory.FindByPaymentProcessor(method.PaymentProcessor);

            if (views != null)
            {
                var url = Url.RouteUrl(views.Settings(method, ControllerContext), RouteValues.From(Request.QueryString));
                return AjaxForm().RedirectTo(url);
            }

            return AjaxForm().RedirectTo(@return);
        }

        private List<SelectListItem> GetAvailableGateways()
        {
            return _gatewayFactory.All()
                                 .OrderBy(x => x.Name)
                                 .Select(x => new SelectListItem
                                 {
                                     Text = x.Name,
                                     Value = x.Name
                                 })
                                 .ToList();
        }
    }
}
