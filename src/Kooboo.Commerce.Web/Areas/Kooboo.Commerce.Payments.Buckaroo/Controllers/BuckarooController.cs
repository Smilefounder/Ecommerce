using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Settings;
using Kooboo.Commerce.Web;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.Buckaroo.Controllers
{
    public class BuckarooController : CommerceController
    {
        private OrderService _orderService;
        private PaymentMethodService _paymentMethodService;

        public BuckarooController(OrderService paymentService, PaymentMethodService paymentMethodService)
        {
            _orderService = paymentService;
            _paymentMethodService = paymentMethodService;
        }

        [Transactional]
        public ActionResult Return(string commerceReturnUrl)
        {
            var paymentId = Convert.ToInt32(Request["add_paymentId"]);
            var payment = _orderService.Payments().ById(paymentId);
            var method = _paymentMethodService.Find(payment.PaymentMethodId);
            var result = ProcessResponse(payment, method.LoadProcessorConfig<BuckarooConfig>());
            _orderService.AcceptPaymentProcessResult(payment, result);

            return Redirect(Url.Payment().DecorateReturn(commerceReturnUrl, payment));
        }

        [Transactional]
        public void Push()
        {
            var paymentId = Convert.ToInt32(Request["add_paymentId"]);
            var payment = _orderService.Payments().ById(paymentId);
            var method = _paymentMethodService.Find(payment.PaymentMethodId);
            var result = ProcessResponse(payment, method.LoadProcessorConfig<BuckarooConfig>());
            _orderService.AcceptPaymentProcessResult(payment, result);
        }

        private PaymentProcessResult ProcessResponse(Payment payment, BuckarooConfig settings)
        {
            var signature = BuckarooUtil.GetSignature(Request.Form, settings.SecretKey);
            if (signature != Request["brq_signature"])
                throw new InvalidOperationException("Invalid response.");

            var statusCode = Request["brq_statuscode"];
            var transactionType = Request["brq_transaction_type"];
            var statusMessage = Request["brq_statusmessage"];
            var transactionId = Request["brq_transactions"];
            var methodId = Request["Brq_payment_method"];

            // Failed / Validation Failure / Technical Failure
            if (statusCode == "490" || statusCode == "491" || statusCode == "492")
            {
                return PaymentProcessResult.Failed(statusCode + ": " + statusMessage);
            }
            // Rejected by the (third party) payment provider
            if (statusCode == "690")
            {
                return PaymentProcessResult.Failed(statusCode + ": " + statusMessage);
            }
            // Cancelled by Customer / Merchant
            if (statusCode == "890" || statusCode == "891")
            {
                return PaymentProcessResult.Cancelled();
            }

            // 190: Success (but might later become reserved)
            if (statusCode != "190")
            {
                return PaymentProcessResult.Pending(null);
            }

            if (methodId == "simplesepadirectdebit")
            {
                // Reserved
                if (transactionType == "C501")
                {
                    return PaymentProcessResult.Pending(transactionId);
                }

                return PaymentProcessResult.Pending(null);
            }
            else if (methodId == "ideal")
            {
                return PaymentProcessResult.Success(transactionId);
            }
            else if (methodId == "paypal")
            {
                return PaymentProcessResult.Success(transactionId);
            }

            throw new NotSupportedException("Not support payment method: " + methodId + ".");
        }
    }
}
