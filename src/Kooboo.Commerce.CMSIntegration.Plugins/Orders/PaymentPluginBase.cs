using Kooboo.Commerce.Api;
using Kooboo.Commerce.Api.Payments;
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
            var order = Site.Commerce().Orders.Query().ById(model.OrderId).FirstOrDefault();
            var returnUrl = ResolveUrl(model.ReturnUrl, ControllerContext);

            var request = new PaymentRequest
            {
                OrderId = model.OrderId,
                Description = "Order #" + model.OrderId,
                Amount = order.Total,
                PaymentMethodId = model.PaymentMethodId,
                ReturnUrl = returnUrl
            };

            var parameters = model.GetPaymentParameters();

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var each in parameters)
                {
                    request.Parameters.Add(each.Key, each.Value);
                }
            }

            var result = Site.Commerce().Orders.Pay(request);
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
