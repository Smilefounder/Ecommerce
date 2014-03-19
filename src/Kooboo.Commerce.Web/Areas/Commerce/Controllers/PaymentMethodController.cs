using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
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
using Kooboo.Commerce.Web.Mvc.Paging;

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
                                 .ToPagedList(page ?? 1, pageSize ?? 50)
                                 .Transform(x =>
                                 {
                                     var model = new PaymentMethodRowModel(x);
                                     var views = _processorViewsFactory.FindByPaymentProcessor(model.PaymentProcessorName);
                                     model.IsConfigurable = views != null;

                                     return model;
                                 });
            return View(methods);
        }

        public ActionResult Create()
        {
            var model = new PaymentMethodEditorModel();
            SetupPaymentProcessors(model);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var method = _paymentMethodService.GetById(id);
            var model = new PaymentMethodEditorModel
            {
                Id = method.Id,
                DisplayName = method.DisplayName,
                PaymentType = method.Type,
                PaymentProcessorName = method.PaymentProcessorName,
                PaymentProcessorMethodId = method.PaymentProcessorMethodId,
                AdditionalFeeChargeMode = method.AdditionalFeeChargeMode,
                AdditionalFeeAmount = method.AdditionalFeeAmount,
                AdditionalFeePercent = method.AdditionalFeePercent,
                IsEnabled = method.IsEnabled,
                IsEdit = true
            };

            SetupPaymentProcessors(model);

            return View(model);
        }

        private void SetupPaymentProcessors(PaymentMethodEditorModel model)
        {
            model.AvailablePaymentProcessors = GetAvailablePaymentProcessors(model.PaymentType);

            var processor = model.AvailablePaymentProcessors.FirstOrDefault(x => x.Name == model.PaymentProcessorName);
            if (processor != null)
            {
                model.AvailablePaymentMethods = processor.SupportedPaymentMethods;
            }
        }

        public ActionResult PaymentProcessors(PaymentMethodType paymentType)
        {
            return JsonNet(GetAvailablePaymentProcessors(paymentType)).UsingClientConvention();
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Settings(PaymentMethodRowModel[] model)
        {
            var method = _paymentMethodService.GetById(model[0].Id);
            var views = _processorViewsFactory.FindByPaymentProcessor(method.PaymentProcessorName);
            var url = Url.RouteUrl(views.Settings(ControllerContext), RouteValues.From(Request.QueryString));

            return AjaxForm().RedirectTo(url);
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Enable(PaymentMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var method = _paymentMethodService.GetById(each.Id);
                _paymentMethodService.Enable(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Disable(PaymentMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var method = _paymentMethodService.GetById(each.Id);
                _paymentMethodService.Disable(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
        public ActionResult Delete(PaymentMethodRowModel[] model)
        {
            foreach (var each in model)
            {
                var method = _paymentMethodService.GetById(each.Id);
                _paymentMethodService.Delete(method);
            }

            return AjaxForm().ReloadPage();
        }

        [HttpPost, HandleAjaxFormError, AutoDbCommit]
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

            var views = _processorViewsFactory.FindByPaymentProcessor(method.PaymentProcessorName);

            if (views != null)
            {
                var url = Url.RouteUrl(views.Settings(ControllerContext), RouteValues.From(Request.QueryString));
                return AjaxForm().RedirectTo(url);
            }

            return AjaxForm().RedirectTo(@return);
        }

        private List<PaymentProcessorModel> GetAvailablePaymentProcessors(PaymentMethodType paymentType)
        {
            var result = new List<PaymentProcessorModel>();
            var processors = _processorFactory.All()
                                              .Where(x => x.SupportedPaymentTypes.Contains(paymentType))
                                              .ToList();

            foreach (var processor in processors)
            {
                var model = new PaymentProcessorModel
                {
                    Name = processor.Name
                };

                if (processor.SupportMultiplePaymentMethods)
                {
                    model.SupportedPaymentMethods = processor.GetSupportedPaymentMethods(paymentType).ToList();
                }

                result.Add(model);
            }

            return result;
        }
    }
}
