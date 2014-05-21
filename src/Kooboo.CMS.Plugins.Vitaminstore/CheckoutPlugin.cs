using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Membership;
using Kooboo.CMS.Sites.View;
using Kooboo.Commerce.API.CmsSite;
using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Plugins.Vitaminstore
{
    public class CheckoutPlugin : ISubmissionPlugin
    {
        public System.Web.Mvc.ActionResult Submit(CMS.Sites.Models.Site site, System.Web.Mvc.ControllerContext controllerContext, CMS.Sites.Models.SubmissionSetting submissionSetting)
        {
            var resultData = new JsonResultData();

            try
            {
                var api = site.Commerce();
                var request = controllerContext.HttpContext.Request;
                var member = controllerContext.HttpContext.Membership().GetMembershipUser();
                var cart = api.ShoppingCarts.ByAccountId(member.UUID).FirstOrDefault();

                var paymentMethodId = Convert.ToInt32(request["paymentMethodId"]);
                var paymentMethod = api.PaymentMethods.ById(paymentMethodId).FirstOrDefault();

                var order = api.Orders.CreateFromShoppingCart(cart.Id, member, true);
                var returnUrl = "/Checkout-Thankyou?orderId=" + order.Id;

                var payment = new PaymentRequest
                {
                    TargetType = PaymentTargetTypes.Order,
                    TargetId = order.Id.ToString(),
                    Description = "Order #" + order.Id,
                    Amount = order.Total + paymentMethod.GetPaymentMethodFee(order.Total),
                    PaymentMethodId = paymentMethodId,
                    ReturnUrl = returnUrl
                };

                foreach (var key in controllerContext.HttpContext.Request.Form.AllKeys)
                {
                    payment.Parameters.Add(key, controllerContext.HttpContext.Request.Form[key]);
                }

                var result = api.Payments.Pay(payment);

                api.ShoppingCarts.ExpireShppingCart(cart.Id);

                resultData.Success = true;
                resultData.Model = new
                {
                    Message = result.Message,
                    RedirectUrl = String.IsNullOrEmpty(result.RedirectUrl) ? returnUrl : result.RedirectUrl,
                    PaymentStatus = result.PaymentStatus.ToString()
                };
            }
            catch (Exception ex)
            {
                resultData.Success = false;
                resultData.AddMessage(ex.Message);
            }

            return new JsonResult { Data = resultData };
        }

        public Dictionary<string, object> Parameters
        {
            get { return null; }
        }
    }
}
