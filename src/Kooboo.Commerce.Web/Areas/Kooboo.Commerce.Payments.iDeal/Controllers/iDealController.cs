using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
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
        private IOrderPaymentService _paymentService;
        private IPaymentMethodService _paymentMethodService;

        public iDealController(
            IKeyValueService keyValueService,
            IOrderPaymentService paymentService,
            IPaymentMethodService paymentMethodService)
        {
            _keyValueService = keyValueService;
            _paymentService = paymentService;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Return(string commerceReturnUrl)
        {
            var iDealTransactionId = Request["transaction_id"];
            var order = _paymentService.GetOrderByExternalPaymentTransactionId(iDealTransactionId, Strings.PaymentProcessorName);

            return Redirect(PaymentReturnUrlUtil.AppendOrderInfoToQueryString(commerceReturnUrl, order));
        }

        [Transactional]
        public void Report()
        {
            var iDealTransactionId = Request["transaction_id"];
            var order = _paymentService.GetOrderByExternalPaymentTransactionId(iDealTransactionId, Strings.PaymentProcessorName);

            var paymentMethod = _paymentMethodService.GetById(order.PaymentMethodId.Value);
            var settings = IDealSettings.FetchFrom(_keyValueService);
            var idealCheck = new IdealCheck(settings.PartnerId, settings.TestMode, iDealTransactionId);

            ProcessPaymentResult result = null;

            if (idealCheck.Error)
            {
                result = ProcessPaymentResult.Failed(iDealTransactionId, idealCheck.ErrorMessage);
            }
            else if (idealCheck.Payed)
            {
                result = ProcessPaymentResult.Paid(iDealTransactionId);
            }

            if (result != null)
            {
                _paymentService.HandlePaymentResult(order, result);
            }
        }
    }
}
