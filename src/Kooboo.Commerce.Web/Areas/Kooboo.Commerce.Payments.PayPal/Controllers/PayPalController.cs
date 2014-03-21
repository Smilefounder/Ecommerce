using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using PayPal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.PayPal.Controllers
{
    public class PayPalController : CommerceControllerBase
    {
        private IKeyValueService _keyValueService;
        private IPaymentService _paymentService;
        private IPaymentMethodService _paymentMethodService;

        public PayPalController(
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

        public ActionResult Cancel(int paymentId, string commerceReturnUrl)
        {
            return Return(paymentId, commerceReturnUrl);
        }

        [AutoDbCommit]
        public void IPN()
        {
            var paymentId = Convert.ToInt32(Request["trackingId"]);
            var payment = _paymentService.GetById(paymentId);
            var paymentMethod = _paymentMethodService.GetById(payment.PaymentMethod.Id);
            var gatewayData = PayPalSettings.FetchFrom(_keyValueService);

            if (gatewayData == null)
                throw new InvalidOperationException("Missing PayPal configuration.");

            var parameters = Request.BinaryRead(Request.ContentLength);

            if (parameters.Length > 0)
            {
                var message = new IPNMessage(PayPalUtil.GetPayPalConfig(gatewayData), parameters);
                if (!message.Validate())
                    throw new InvalidOperationException("Invalid PalPal IPN request.");

                var payPalPaymentStatus = Request["payment_status"];
                var payPalTransactionId = Request["txn_id"];
                var paymentStatus = PayPalUtil.GetPaymentStatus(payPalPaymentStatus);

                ProcessPaymentResult result = null;

                if (paymentStatus == PaymentStatus.Success)
                {
                    result = ProcessPaymentResult.Success(payPalTransactionId);
                }
                else if (paymentStatus == PaymentStatus.Failed)
                {
                    result = ProcessPaymentResult.Failed(null, payPalTransactionId);
                }

                if (result != null)
                {
                    payment.HandlePaymentResult(result);
                }
            }
        }
    }
}
