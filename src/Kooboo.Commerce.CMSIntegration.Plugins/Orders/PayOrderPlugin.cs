using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.CMSIntegration.Plugins.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Orders
{
    public class PayOrderPlugin : SubmissionPluginBase<PayOrderModel>
    {
        protected override SubmissionExecuteResult Execute(PayOrderModel model)
        {
            var order = Site.Commerce().Orders.ById(model.OrderId).FirstOrDefault();
            var paymentMethod = Site.Commerce().PaymentMethods.ById(model.PaymentMethodId).FirstOrDefault();

            var returnUrl = ResolveUrl(model.SuccessUrl, ControllerContext);

            var payment = new PaymentRequest
            {
                TargetType = PaymentTargetTypes.Order,
                TargetId = model.OrderId.ToString(),
                Description = "Order #" + model.OrderId,
                Amount = order.Total + paymentMethod.GetPaymentMethodFee(order.Total),
                PaymentMethodId = model.PaymentMethodId,
                ReturnUrl = returnUrl
            };

            if (model.PaymentParameters != null && model.PaymentParameters.Count > 0)
            {
                foreach (var each in model.PaymentParameters)
                {
                    payment.Parameters.Add(each.Key, each.Value);
                }
            }

            var result = Site.Commerce().Payments.Pay(payment);

            return new SubmissionExecuteResult
            {
                Data = new PayOrderResult
                {
                    PaymentStatus = result.PaymentStatus.ToString(),
                    Message = result.Message,
                    RedirectUrl = String.IsNullOrEmpty(result.RedirectUrl) ? returnUrl : result.RedirectUrl
                }
            };
        }
    }
}
