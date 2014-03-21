using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class PaymentController : CommerceControllerBase
    {
        private ISettingService _settingService;
        private IPaymentService _paymentService;
        private IPaymentMethodService _paymentMethodService;
        private IPaymentProcessorFactory _paymentProcessorFactory;

        public PaymentController(
            ISettingService settingService,
            IPaymentService paymentService,
            IPaymentMethodService paymentMethodService,
            IPaymentProcessorFactory paymentProcessorFactory)
        {
            _settingService = settingService;
            _paymentService = paymentService;
            _paymentMethodService = paymentMethodService;
            _paymentProcessorFactory = paymentProcessorFactory;
        }

        [AutoDbCommit]
        public ActionResult Gateway(int paymentId, string returnUrl)
        {
            var settings = _settingService.GetStoreSetting();
            var payment = _paymentService.GetById(paymentId);
            var paymentMethod = _paymentMethodService.GetById(payment.PaymentMethod.Id);
            var commerceName = CommerceContext.CurrentInstance.Name;

            var paymentRequest = new ProcessPaymentRequest(payment)
            {
                CurrencyCode = settings.CurrencyISOCode,
                //CreditCardInfo = model.CreditCardInfo,
                ReturnUrl = returnUrl
            };

            var processor = _paymentProcessorFactory.FindByName(paymentMethod.PaymentProcessorName);
            var result = processor.ProcessPayment(paymentRequest);

            if (result.PaymentStatus == PaymentStatus.Success)
            {
                // Already done
                payment.HandlePaymentResult(result);
                return Redirect(paymentRequest.ReturnUrl);
            }
            else
            {
                return result.NextAction;
            }
        }
    }
}