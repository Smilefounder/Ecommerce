using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Membership;
using Kooboo.CMS.Common;

namespace Kooboo.Commerce.API
{
    public class AddToCartPlugIn : ISubmissionPlugin
    {
        public Dictionary<string, object> Parameters
        {
            get { return new Dictionary<string, object>() { { "ProductPriceId", "{ProductPriceId}" }, { "Quantity", "{Quantity}" } }; }
        }

        public ActionResult Submit(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            JsonResultData resultData = new JsonResultData();

            string commerceInstance = site.GetCommerceName();
            string language = site.GetLanguage();

            try
            {
                var form = controllerContext.HttpContext.Request.Form;

                int productPriceId = Convert.ToInt32(form["productPriceId"]);
                int quantity = Convert.ToInt32(form["quantity"]);

                if (controllerContext.HttpContext.Session["CartGuest"] == null)
                    controllerContext.HttpContext.Session["CartGuest"] = Guid.NewGuid().ToString();

                Guid guestId = new Guid(controllerContext.HttpContext.Session["CartGuest"].ToString());
                int? customerId = null;
                var memberAuth = controllerContext.HttpContext.Membership();
                var member = memberAuth.GetMember();
                if (member.Identity.IsAuthenticated)
                {
                    var customer = site.Commerce().GetCustomerByAccountId(commerceInstance, language, Convert.ToInt32(member.Identity.Name));
                    if (customer != null)
                        customerId = customer.Id;
                }

                if (site.Commerce().AddToCart(commerceInstance, language, guestId, customerId, productPriceId, quantity))
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
