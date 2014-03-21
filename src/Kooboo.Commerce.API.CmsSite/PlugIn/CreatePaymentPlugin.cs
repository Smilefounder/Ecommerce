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
    public class CreatePaymentPlugin : ISubmissionPlugin
    {
        public Dictionary<string, object> Parameters
        {
            get { return null; }
        }

        public System.Web.Mvc.ActionResult Submit(CMS.Sites.Models.Site site, System.Web.Mvc.ControllerContext controllerContext, CMS.Sites.Models.SubmissionSetting submissionSetting)
        {
            var resultData = new JsonResultData();

            try
            {
                var commerceService = site.Commerce();
                var form = controllerContext.HttpContext.Request.Form;
                var orderId = Convert.ToInt32(form["orderId"]);
                var paymentMethodId = Convert.ToInt32(form["paymentMethodId"]);
                var returnUrl = form["returnUrl"];
                var order = commerceService.Orders.ById(orderId).FirstOrDefault();

                var result = commerceService.Payments.Create(new CreatePaymentRequest
                {
                    TargetType = PaymentTargetTypes.Order,
                    TargetId = orderId.ToString(),
                    Description = "Order #" + order.Id,
                    Amount = order.Total,
                    PaymentMethodId = paymentMethodId,
                    ReturnUrl = returnUrl
                });

                resultData.Success = true;
                resultData.Model = result;
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
