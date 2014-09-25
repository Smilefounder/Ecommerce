using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Web;
using Kooboo.Commerce.Web.Framework.Mvc;
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
        private SettingService _keyValueService;
        private PaymentService _paymentService;
        private PaymentMethodService _paymentMethodService;

        public iDealController(
            SettingService keyValueService,
            PaymentService paymentService,
            PaymentMethodService paymentMethodService)
        {
            _keyValueService = keyValueService;
            _paymentService = paymentService;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Return(int paymentId, string commerceReturnUrl)
        {
            var payment = _paymentService.Find(paymentId);
            return Redirect(Url.Payment().DecorateReturn(commerceReturnUrl, payment));
        }

        [Transactional]
        public void Report()
        {
            var iDealTransactionId = Request["transaction_id"];
            var payment = _paymentService.Query().ByThirdPartyTransactionId(iDealTransactionId, "iDeal");

            var paymentMethod = _paymentMethodService.Find(payment.PaymentMethodId);
            var settings = paymentMethod.LoadProcessorConfig<IDealConfig>();
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
                _paymentService.AcceptProcessResult(payment, result);
            }
        }
    }
}
