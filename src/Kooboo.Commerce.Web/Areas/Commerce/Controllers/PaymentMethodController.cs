using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Web.Mvc;
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
        private IPaymentProcessorFactory _processorFactory;
        private IPaymentProcessorViewsFactory _processorViewsFactory;
        private IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(
            IPaymentProcessorFactory processorFactory,
            IPaymentProcessorViewsFactory processorViewFactory,
            IPaymentMethodService paymentMethodService)
        {
            _processorFactory = processorFactory;
            _processorViewsFactory = processorViewFactory;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            var methods = _paymentMethodService.Query()
                                 .OrderByDescending(x => x.Id)
                                 .ToPagedList(page, pageSize)
                                 .Transform(x =>
                                 {
                                     var model = new PaymentMethodRowModel(x);
                                     var views = _processorViewsFactory.FindByPaymentProcessor(model.PaymentProcessorName);
                                     model.IsConfigurable = views != null;

                                     return model;
                                 });
            return View(methods);
        }

        public ActionResult Steps(int step)
        {
            ViewBag.Step = step;
            return PartialView();
        }

        public ActionResult BasicInfo(int? id)
        {
            var model = new PaymentMethodEditorModel
            {
                AvailablePaymentProcessors = GetAvailablePaymentProcessors()
            };

            if (id != null)
            {
                var method = _paymentMethodService.GetById(id.Value);
                model.Id = method.Id;
                model.DisplayName = method.DisplayName;
                model.UniqueId = method.UniqueId;
                model.PaymentProcessorName = method.PaymentProcessorName;
                model.AdditionalFeeChargeMode = method.AdditionalFeeChargeMode;
                model.AdditionalFeeAmount = method.AdditionalFeeAmount;
                model.AdditionalFeePercent = method.AdditionalFeePercent;
            }

            return View(model);
        }

        public ActionResult Complete(int id)
        {
            var method = _paymentMethodService.GetById(id);
            return View(method);
        }

        private IList<PaymentProcessorModel> GetAvailablePaymentProcessors()
        {
            return _processorFactory.All().Select(x => new PaymentProcessorModel
            {
                Name = x.Name
            })
            .ToList();
        }


        [HttpPost, HandleAjaxFormError]
        public ActionResult Settings(PaymentMethodRowModel[] model)
        {
            var method = _paymentMethodService.GetById(model[0].Id);
            var views = _processorViewsFactory.FindByPaymentProcessor(method.PaymentProcessorName);
            var url = Url.RouteUrl(views.Settings(method, ControllerContext), RouteValues.From(Request.QueryString).Merge("id", method.Id));

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

        [HttpPost, Transactional]
        public void EnablePaymentMethod(int id)
        {
            var method = _paymentMethodService.GetById(id);
            _paymentMethodService.Enable(method);
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

            var views = _processorViewsFactory.FindByPaymentProcessor(method.PaymentProcessorName);

            if (views != null)
            {
                var url = Url.RouteUrl(views.Settings(method, ControllerContext), RouteValues.From(Request.QueryString).Merge("id", model.Id));
                return AjaxForm().RedirectTo(url);
            }

            return AjaxForm().RedirectTo(@return);
        }
    }
}
