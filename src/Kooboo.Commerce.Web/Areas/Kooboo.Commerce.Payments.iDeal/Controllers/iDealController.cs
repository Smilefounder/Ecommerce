using Kooboo.Commerce.Data;
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
        public ActionResult Return(int paymentId, string commerceReturnUrl)
        {
            var orderService = new OrderService(CommerceInstance.Current);
            var payment = orderService.Payments().FirstOrDefault(it => it.Id == paymentId);
            return Redirect(Url.Payment().DecorateReturn(commerceReturnUrl, payment));
        }

        [Transactional]
        public void Report()
        {
            var iDealTransactionId = Request["transaction_id"];

            var orderService = new OrderService(CommerceInstance.Current);
            var paymentMethodService = new PaymentMethodService(CommerceInstance.Current);

            var payment = orderService.Payments().ByThirdPartyTransactionId(iDealTransactionId, "iDeal");

            var paymentMethod = paymentMethodService.Find(payment.PaymentMethodId);
            var settings = paymentMethod.LoadProcessorConfig<IDealConfig>();
            var idealCheck = new IdealCheck(settings.PartnerId, settings.TestMode, iDealTransactionId);

            PaymentProcessResult result = null;

            if (idealCheck.Error)
            {
                result = PaymentProcessResult.Failed(idealCheck.ErrorMessage, iDealTransactionId);
            }
            else if (idealCheck.Payed)
            {
                result = PaymentProcessResult.Success(iDealTransactionId);
            }

            if (result != null)
            {
                orderService.AcceptPaymentProcessResult(payment, result);
            }
        }
    }
}
