using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Web.Areas.Commerce.Models.PaymentMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Commerce.Web.Areas.Commerce.Models;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class PaymentMethodController : CommerceController
    {
        private IPaymentProcessorProvider _processorProvider;
        private PaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentProcessorProvider processorProvider, PaymentMethodService paymentMethodService)
        {
            _processorProvider = processorProvider;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            var methods = _paymentMethodService.Query()
                                 .OrderByDescending(x => x.Id)
                                 .Paginate(page - 1, pageSize)
                                 .Transform(x => new PaymentMethodRowModel(x))
                                 .ToPagedList();
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
                model.Name = method.Name;
                model.UserKey = method.UserKey;
                model.ProcessorName = method.ProcessorName;
                model.AdditionalFeeChargeMode = method.AdditionalFeeChargeMode;
                model.AdditionalFeeAmount = method.AdditionalFeeAmount;
                model.AdditionalFeePercent = method.AdditionalFeePercent;
            }

            return View(model);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult BasicInfo(PaymentMethodEditorModel model, string @return)
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

            if (model.Id > 0)
            {
                CurrentInstance.Database.SaveChanges();
            }

            string redirectUrl = null;
            var processor = _processorProvider.FindByName(method.ProcessorName);

            var editor = processor as IHasCustomPaymentProcessorConfigEditor;
            if (editor != null || processor.ConfigType != null)
            {
                redirectUrl = Url.Action("Processor", RouteValues.From(Request.QueryString).Merge("id", method.Id));
            }
            else
            {
                redirectUrl = Url.Action("Complete", RouteValues.From(Request.QueryString).Merge("id", method.Id));
            }

            return AjaxForm().RedirectTo(redirectUrl);
        }

        public ActionResult Processor(int id)
        {
            var method = _paymentMethodService.GetById(id);
            var processor = _processorProvider.FindByName(method.ProcessorName);

            var editorModel = new PaymentProcessorConfigEditorModel
            {
                PaymentMethodId = method.Id,
                Config = method.LoadProcessorConfig(processor.ConfigType) ?? Activator.CreateInstance(processor.ConfigType)
            };

            ViewBag.Processor = processor;
            ViewBag.ProcessorConfigEditorModel = editorModel;

            return View(method);
        }

        [HttpPost, HandleAjaxError, Transactional]
        public void UpdateProcessorConfig(int paymentMethodId, [ModelBinder(typeof(BindingTypeAwareModelBinder))]object config)
        {
            var method = _paymentMethodService.GetById(paymentMethodId);
            method.UpdateProcessorConfig(config);
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
    }
}
