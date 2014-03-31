using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Extension;
using Kooboo.Commerce.API.Prices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.API.CmsSite.PlugIn
{
    public class CalculatePricePlugin : ISubmissionPlugin
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
                var commerce = site.Commerce();
                var request = controllerContext.HttpContext.Request;
                var orderId = Convert.ToInt32(request["orderId"]);
                int? paymentMethodId = null;
                if (!String.IsNullOrEmpty(request["paymentMethodId"])){
                    paymentMethodId = Convert.ToInt32(request["paymentMethodId"]);
                }

                var result = commerce.Prices.CalculateOrderPrice(new CalculateOrderPriceRequest
                {
                    OrderId = orderId,
                    PaymentMethodId = paymentMethodId
                });

                resultData.Success = true;
                resultData.Model = result;
            }
            catch (Exception ex)
            {
                resultData.Success = false;
                resultData.AddException(ex);
            }

            return new JsonResult { Data = resultData };
        }
    }
}
