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
using Kooboo.Commerce.Web.Areas.Commerce.Models;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class PaymentMethodController : CommerceControllerBase
    {
        private IPaymentProcessorProvider _processorProvider;
        private IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentProcessorProvider processorProvider, IPaymentMethodService paymentMethodService)
        {
            _processorProvider = processorProvider;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            var methods = _paymentMethodService.Query()
                                 .OrderByDescending(x => x.Id)
                                 .ToPagedList(page, pageSize)
                                 .Transform(x => new PaymentMethodRowModel(x));
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
            return _processorProvider.All().Select(x => new PaymentProcessorModel
            {
                Name = x.Name
            })
            .ToList();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Enable(PaymentMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var method = _paymentMethodService.GetById(each.Id);
                method.Enable();
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, Transactional]
        public void EnablePaymentMethod(int id)
        {
            var method = _paymentMethodService.GetById(id);
            method.Enable();
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult Disable(PaymentMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var method = _paymentMethodService.GetById(each.Id);
                method.Disable();
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

        public ActionResult Processor(int id)
        {
            var method = _paymentMethodService.GetById(id);
            var processor = _processorProvider.FindByName(method.PaymentProcessorName);
            ViewBag.Processor = processor;
            return View(method);
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

            var processor = _processorProvider.FindByName(method.PaymentProcessorName);
            var editor = processor.GetEditor();

            string redirectUrl = null;

            if (editor != null)
            {
                redirectUrl = Url.Action("Processor", RouteValues.From(Request.QueryString).Merge("id", method.Id));
            }
            else
            {
                redirectUrl = Url.Action("Complete", RouteValues.From(Request.QueryString).Merge("id", method.Id));
            }

            return AjaxForm().RedirectTo(redirectUrl);
        }
    }
}
