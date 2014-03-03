using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Settings.Services;
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
        private IOrderService _orderService;
        private IOrderPaymentService _orderPaymentService;
        private IKeyValueService _keyValueService;

        public BuckarooController(IOrderService orderService, IOrderPaymentService orderPaymentService, IKeyValueService keyValueService)
        {
            _orderService = orderService;
            _orderPaymentService = orderPaymentService;
            _keyValueService = keyValueService;
        }

        [Transactional]
        public ActionResult Return(string commerceReturnUrl)
        {
            var orderId = Convert.ToInt32(Request["add_orderId"]);
            var order = _orderService.GetById(orderId);
            var result = ProcessResponse(order, BuckarooSettings.FetchFrom(_keyValueService));
            _orderPaymentService.HandlePaymentResult(order, result);

            return Redirect(PaymentReturnUrlUtil.AppendOrderInfoToQueryString(commerceReturnUrl, order));
        }

        [Transactional]
        public void Push()
        {
            var orderId = Convert.ToInt32(Request["add_orderId"]);
            var order = _orderService.GetById(orderId);
            var result = ProcessResponse(order, BuckarooSettings.FetchFrom(_keyValueService));
            _orderPaymentService.HandlePaymentResult(order, result);
        }

        private ProcessPaymentResult ProcessResponse(Order order, BuckarooSettings settings)
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
