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

        public ActionResult ReturnCancel(string commerceReturnUrl)
        {
            return Redirect(commerceReturnUrl);
        }

        public ActionResult ReturnError(string commerceReturnUrl)
        {
            return Redirect(commerceReturnUrl);
        }

        public ActionResult ReturnReject(string commerceReturnUrl)
        {
            return Redirect(commerceReturnUrl);
        }

        [Transactional]
        public void Push()
        {
            var orderId = Convert.ToInt32(Request["add_orderId"]);
            var order = _orderService.GetById(orderId);
            var result = ProcessResponse(order, BuckarooSettings.FetchFrom(_keyValueService));
            _orderPaymentService.HandlePaymentResult(order, result);
        }

        static readonly HashSet<string> _refundTransactionTypes = new HashSet<string>
        {
            "V090", "V110", "V066", "V067", "V068", "V070", "V072", "V078", "V079", "V080", "V082", "V085", "C102", "C121", "C543", "C101"
        };

        static readonly HashSet<string> _reservedTransactionTypes = new HashSet<string>
        {
            "C562", "C544", "V111"
        };

        private ProcessPaymentResult ProcessResponse(Order order, BuckarooSettings settings)
        {
            var signature = BuckarooUtil.GetSignature(Request.Form, settings.SecretKey);
            if (signature != Request["brq_signature"])
                throw new InvalidOperationException("Invalid response.");

            var statusCode = Request["brq_statuscode"];
            var statusMessage = Request["brq_statusmessage"];
            var transactionId = Request["brq_transactions"];

            if (statusCode != "190")
            {
                return ProcessPaymentResult.Failed(statusCode + ": " + statusMessage);
            }

            var transactionType = Request["brq_transaction_type"];

            if (_refundTransactionTypes.Contains(transactionType))
            {
                return ProcessPaymentResult.Pending(null);
            }

            if (transactionType == "C002") // Direct debit collect
            {
                return ProcessPaymentResult.Pending(null);
            }

            if (transactionType == "C462")
            {
                return ProcessPaymentResult.Success();
                // "IJ PG NL Pay-out";
            }

            var amount = Decimal.Parse(Request["brq_amount"], CultureInfo.InvariantCulture);
            if (amount != order.Total)
                throw new InvalidOperationException("Incorrect amount.");

            var methodId = Request["Brq_payment_method"];

            if (methodId == "paymentguarantee" || transactionType == "I242" || transactionType == "I243")//Online giro
            {
                return ProcessPaymentResult.Success();
            }

            return ProcessPaymentResult.Success();
        }
    }
}
