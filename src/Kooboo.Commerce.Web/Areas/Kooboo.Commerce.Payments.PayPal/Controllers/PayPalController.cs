using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
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
        private IOrderService _orderService;
        private IOrderPaymentService _orderPaymentService;
        private IPaymentMethodService _paymentMethodService;

        public PayPalController(
            IOrderService orderService,
            IOrderPaymentService orderPaymentService,
            IPaymentMethodService paymentMethodService)
        {
            _orderService = orderService;
            _orderPaymentService = orderPaymentService;
            _paymentMethodService = paymentMethodService;
        }

        public ActionResult Return(string commerceReturnUrl)
        {
            return Redirect(commerceReturnUrl);
        }

        public ActionResult Cancel(string commerceReturnUrl)
        {
            return Return(commerceReturnUrl);
        }

        public void IPN()
        {
            var orderId = Convert.ToInt32(Request["trackingId"]);
            var order = _orderService.GetById(orderId);
            var paymentMethod = _paymentMethodService.GetById(order.PaymentMethodId.Value);
            var gatewayData = PayPalPaymentGatewayData.Deserialize(paymentMethod.PaymentGatewayData);

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
                    result = ProcessPaymentResult.Paid(payPalTransactionId);
                }
                else if (paymentStatus == PaymentStatus.Failed)
                {
                    result = ProcessPaymentResult.Failed(payPalTransactionId, null);
                }

                if (result != null)
                {
                    _orderPaymentService.HandlePaymentResult(order, result);
                    CommerceContext.CurrentInstance.Database.SaveChanges();
                }
            }
        }
    }
}
