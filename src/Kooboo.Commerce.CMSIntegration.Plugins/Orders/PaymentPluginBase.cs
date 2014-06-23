using Kooboo.Commerce.API.Payments;
using Kooboo.Commerce.CMSIntegration.Plugins.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Orders
{
    public abstract class PaymentPluginBase<TModel> : SubmissionPluginBase<TModel>
        where TModel : PaymentModelBase, new()
    {
        protected override SubmissionExecuteResult Execute(TModel model)
        {
            var order = Site.Commerce().Orders.ById(model.OrderId).FirstOrDefault();
            var paymentMethod = Site.Commerce().PaymentMethods.ById(model.PaymentMethodId).FirstOrDefault();

            var returnUrl = ResolveUrl(model.ReturnUrl, ControllerContext);

            // TODO: Don't calculate payment method cost here, calculate in the commerce side
            var payment = new PaymentRequest
            {
                TargetType = PaymentTargetTypes.Order,
                TargetId = model.OrderId.ToString(),
                Description = "Order #" + model.OrderId,
                Amount = order.Total + paymentMethod.GetPaymentMethodFee(order.Total),
                PaymentMethodId = model.PaymentMethodId,
                ReturnUrl = returnUrl
            };

            var parameters = model.GetPaymentParameters();

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var each in parameters)
                {
                    payment.Parameters.Add(each.Key, each.Value);
                }
            }

            var result = Site.Commerce().Payments.Pay(payment);
            var data = new PayOrderResult
            {
                PaymentStatus = result.PaymentStatus.ToString(),
                Message = result.Message,
                RedirectUrl = String.IsNullOrEmpty(result.RedirectUrl) ? returnUrl : result.RedirectUrl
            };

            return new SubmissionExecuteResult
            {
                RedirectUrl = data.RedirectUrl,
                Data = data
            };
        }
    }
}
