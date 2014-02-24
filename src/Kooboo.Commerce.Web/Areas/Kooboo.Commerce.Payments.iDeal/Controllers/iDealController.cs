using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
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
        private IOrderPaymentService _paymentService;
        private IPaymentMethodService _paymentMethodService;

        public iDealController(
            IOrderPaymentService paymentService,
            IPaymentMethodService paymentMethodService)
        {
            _paymentService = paymentService;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Return(string commerceReturnUrl)
        {
            var iDealTransactionId = Request["transaction_id"];
            var order = _paymentService.GetOrderByExternalPaymentTransactionId(iDealTransactionId, Strings.PaymentGatewayName);

            return Redirect(PaymentReturnUrlUtil.AppendOrderInfoToQueryString(commerceReturnUrl, order));
        }

        [Transactional]
        public void Report()
        {
            var iDealTransactionId = Request["transaction_id"];
            var order = _paymentService.GetOrderByExternalPaymentTransactionId(iDealTransactionId, Strings.PaymentGatewayName);

            var paymentMethod = _paymentMethodService.GetById(order.PaymentMethodId.Value);
            var settings = IDealPaymentGatewayData.Deserialize(paymentMethod.PaymentGatewayData);
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
                _paymentService.AcceptPaymentResult(order, result);
            }
        }
    }
}
