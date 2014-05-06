using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.Commerce.API.CmsSite;
using Kooboo.Commerce.API.ShoppingCarts;

namespace Kooboo.CMS.Plugins.Vitaminstore
{
    public class CartSubmissionPlugin : ISubmissionPlugin
    {
        public Dictionary<string, object> Parameters
        {
            get
            {
                return null;
            }
        }

        public System.Web.Mvc.ActionResult Submit(CMS.Sites.Models.Site site, System.Web.Mvc.ControllerContext controllerContext, CMS.Sites.Models.SubmissionSetting submissionSetting)
        {
            var request = controllerContext.HttpContext.Request;
            var action = request["action"];

            var jsonResultData = new JsonResultData();
            object result = null;

            try
            {
                if (action == "cartinfo")
                {
                    result = CartInfo(site, controllerContext, submissionSetting);
                }
                else if (action == "additem")
                {
                    result = AddItem(site, controllerContext, submissionSetting);
                }

                jsonResultData.Success = true;
                jsonResultData.Model = result;
            }
            catch (Exception ex)
            {
                jsonResultData.Success = false;
                jsonResultData.AddException(ex);
            }

            return new JsonResult { Data = jsonResultData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private ShoppingCart CartInfo(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            ShoppingCart cart = null;

            var api = site.Commerce();
            // TODO: Session might be lost within a browser session
            var sessionId = controllerContext.HttpContext.Session.SessionID;
            var user = controllerContext.HttpContext.Membership().GetMembershipUser();
            if (user == null)
            {
                cart = api.ShoppingCarts.BySessionId(sessionId).FirstOrDefault();
            }
            else
            {
                cart = api.ShoppingCarts.ByAccountId(user.UUID).FirstOrDefault();
            }

            return cart ?? new ShoppingCart
            {
                SessionId = sessionId
            };
        }

        private ShoppingCart AddItem(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            var httpContext = controllerContext.HttpContext;
            var request = httpContext.Request;

            var productPriceId = Convert.ToInt32(request.Form["productPriceId"]);
            var quantity = Convert.ToInt32(request.Form["quantity"]);

            var api = site.Commerce();
            var sessionId = httpContext.Session.SessionID;
            var member = httpContext.Membership().GetMembershipUser();
            var accountId = member == null ? null : member.UUID;

            if (!api.ShoppingCarts.AddToCart(sessionId, accountId, productPriceId, quantity))
                throw new Exception("Add to cart failed, please try again later.");

            return CartInfo(site, controllerContext, submissionSetting);
        }
    }
}
