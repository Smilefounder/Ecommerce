using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Membership;
using Kooboo.CMS.Common;

namespace Kooboo.Commerce.API.PlugIn
{
    public class AddToCartPlugIn : ISubmissionPlugin
    {
        public Dictionary<string, object> Parameters
        {
            get { return null; }
        }

        public ActionResult Submit(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            JsonResultData resultData = new JsonResultData();

            try
            {
                var form = controllerContext.HttpContext.Request.Form;

                int productPriceId = Convert.ToInt32(form["productPriceId"]);
                int quantity = Convert.ToInt32(form["quantity"]);
                var commerService = site.Commerce();
                string sessionId = controllerContext.HttpContext.Session.SessionID;
                var memberAuth = controllerContext.HttpContext.Membership();
                var member = memberAuth.GetMembershipUser();
                if (commerService.Cart.AddToCart(sessionId, member.UUID, productPriceId, quantity))
                {
                    resultData.Success = true;
                    resultData.AddMessage("Successfully add to cart.");
                }
                else
                {
                    resultData.Success = false;
                    resultData.AddMessage("Add to cart failed, please try again later.");
                }
            }
            catch(Exception ex)
            {
                resultData.Success = false;
                resultData.AddMessage(ex.Message);
            }
            return new JsonResult() { Data = resultData };
        }
    }
}
