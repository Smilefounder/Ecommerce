using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Extension;
using Kooboo.Commerce.API.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.API.CmsSite.PlugIn
{
    /// <summary>
    /// payment request form plugin 
    /// it is used in the kooboo cms view to let the user post back the payment form at front-site.
    /// </summary>
    public class PayOrderPlugIn : ISubmissionPlugin
    {
        /// <summary>
        /// predefined parameters, these parameters will not be rendered at the page/view.
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get { return null; }
        }

        /// <summary>
        /// handle the post back form
        /// </summary>
        /// <param name="site">kooboo cms current site</param>
        /// <param name="controllerContext">controller runtime context</param>
        /// <param name="submissionSetting">submission settings of the parameters</param>
        /// <returns>response result</returns>
        public System.Web.Mvc.ActionResult Submit(CMS.Sites.Models.Site site, System.Web.Mvc.ControllerContext controllerContext, CMS.Sites.Models.SubmissionSetting submissionSetting)
        {
            var resultData = new JsonResultData();

            try
            {
                var commerceService = site.Commerce();
                var form = controllerContext.HttpContext.Request.Form;
                var orderId = Convert.ToInt32(form["orderId"]);
                var paymentMethodId = Convert.ToInt32(form["paymentMethodId"]);
                var paymentMethod = commerceService.PaymentMethods.ById(paymentMethodId).FirstOrDefault();
                var returnUrl = form["returnUrl"];
                var order = commerceService.Orders.ById(orderId).FirstOrDefault();

                var request = new PaymentRequest
                {
                    TargetType = PaymentTargetTypes.Order,
                    TargetId = orderId.ToString(),
                    Description = "Order #" + order.Id,
                    Amount = order.Total,
                    PaymentMethodId = paymentMethodId,
                    ReturnUrl = returnUrl
                };

                foreach (var each in paymentMethod.PaymentProcessorParameterDescriptors)
                {
                    var paramValue = controllerContext.HttpContext.Request.Form[each.ParameterName];
                    if (paramValue != null)
                    {
                        request.Parameters.Add(each.ParameterName, paramValue);
                    }
                }

                var result = commerceService.Payments.Pay(request);

                resultData.Success = true;
                resultData.Model = new
                {
                    Message = result.Message,
                    RedirectUrl = result.RedirectUrl,
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
    }
}
