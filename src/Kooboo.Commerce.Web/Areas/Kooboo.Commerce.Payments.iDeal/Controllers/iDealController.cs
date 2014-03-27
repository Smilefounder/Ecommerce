using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web;
using Kooboo.Commerce.Web.Mvc;
using Mollie.iDEAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.iDeal.Controllers
{
    public class iDealController : Controller
    {
        private IKeyValueService _keyValueService;
        private IPaymentService _paymentService;
        private IPaymentMethodService _paymentMethodService;

        public iDealController(
            IKeyValueService keyValueService,
            IPaymentService paymentService,
            IPaymentMethodService paymentMethodService)
        {
            _keyValueService = keyValueService;
            _paymentService = paymentService;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Return(int paymentId, string commerceReturnUrl)
        {
            var payment = _paymentService.GetById(paymentId);
            return Redirect(Url.Payment().DecorateReturn(commerceReturnUrl, payment));
        }

        [AutoDbCommit]
        public void Report()
        {
            var iDealTransactionId = Request["transaction_id"];
            var payment = _paymentService.Query()
                                         .ForOrders()
                                         .ByThirdPartyTransactionId(iDealTransactionId, Strings.PaymentProcessorName);

            var paymentMethod = _paymentMethodService.GetById(payment.PaymentMethod.Id);
            var settings = IDealSettings.Deserialize(paymentMethod.PaymentProcessorData);
            var idealCheck = new IdealCheck(settings.PartnerId, settings.TestMode, iDealTransactionId);

            ProcessPaymentResult result = null;

            if (idealCheck.Error)
            {
                result = ProcessPaymentResult.Failed(idealCheck.ErrorMessage, iDealTransactionId);
            }
            else if (idealCheck.Payed)
            {
                result = ProcessPaymentResult.Success(iDealTransactionId);
            }

            if (result != null)
            {
                payment.HandlePaymentResult(result);
            }
        }
    }
}
