using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Membership;
using Kooboo.CMS.Common;

namespace Kooboo.Commerce.API.CmsSite.PlugIn
{
    /// <summary>
    /// add to cart form plugin 
    /// it is used in the kooboo cms view to let the user post back the shopping items to cart at front-site.
    /// </summary>
    public class AddToCartPlugIn : ISubmissionPlugin
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
        public ActionResult Submit(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            JsonResultData resultData = new JsonResultData();

            try
            {
                var form = controllerContext.HttpContext.Request.Form;

                // get shopping cart info from form.
                int productPriceId = Convert.ToInt32(form["productPriceId"]);
                int quantity = Convert.ToInt32(form["quantity"]);
                // get commerce instance
                var commerService = site.Commerce();
                string sessionId = controllerContext.HttpContext.Session.SessionID;
                var memberAuth = controllerContext.HttpContext.Membership();
                var member = memberAuth.GetMembershipUser();
                // call shopping cart api to add to cart
                if (commerService.ShoppingCarts.AddToCart(sessionId, member == null ? null : member.UUID, productPriceId, quantity))
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
