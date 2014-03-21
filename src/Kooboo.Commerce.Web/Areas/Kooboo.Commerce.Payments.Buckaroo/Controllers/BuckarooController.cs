using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Web;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.Buckaroo.Controllers
{
    public class BuckarooController : CommerceControllerBase
    {
        private IPaymentService _paymentService;
        private IKeyValueService _keyValueService;

        public BuckarooController(IPaymentService paymentService, IKeyValueService keyValueService)
        {
            _paymentService = paymentService;
            _keyValueService = keyValueService;
        }

        [AutoDbCommit]
        public ActionResult Return(string commerceReturnUrl)
        {
            var paymentId = Convert.ToInt32(Request["add_paymentId"]);
            var payment = _paymentService.GetById(paymentId);
            var result = ProcessResponse(payment, BuckarooSettings.FetchFrom(_keyValueService));
            payment.HandlePaymentResult(result);

            return Redirect(Url.Payment().DecorateReturn(commerceReturnUrl, payment));
        }

        [AutoDbCommit]
        public void Push()
        {
            var paymentId = Convert.ToInt32(Request["add_paymentId"]);
            var payment = _paymentService.GetById(paymentId);
            var result = ProcessResponse(payment, BuckarooSettings.FetchFrom(_keyValueService));
            payment.HandlePaymentResult(result);
        }

        private ProcessPaymentResult ProcessResponse(Payment payment, BuckarooSettings settings)
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
                return ProcessPaymentResult.Failed(statusCode + ": " + statusMessage);
            }
            // Rejected by the (third party) payment provider
            if (statusCode == "690")
            {
                return ProcessPaymentResult.Failed(statusCode + ": " + statusMessage);
            }
            // Cancelled by Customer / Merchant
            if (statusCode == "890" || statusCode == "891")
            {
                return ProcessPaymentResult.Cancelled();
            }

            // 190: Success (but might later become reserved)
            if (statusCode != "190")
            {
                return ProcessPaymentResult.Pending(null);
            }

            if (methodId == "simplesepadirectdebit")
            {
                // Reserved
                if (transactionType == "C501")
                {
                    return ProcessPaymentResult.Reserved(transactionId);
                }

                return ProcessPaymentResult.Pending(null);
            }
            else if (methodId == "ideal")
            {
                return ProcessPaymentResult.Success(transactionId);
            }
            else if (methodId == "paypal")
            {
                return ProcessPaymentResult.Success(transactionId);
            }

            throw new NotSupportedException("Not support payment method: " + methodId + ".");
        }
    }
}
