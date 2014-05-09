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
using Kooboo.Commerce.API.Products;

namespace Kooboo.CMS.Plugins.Vitaminstore
{
    public class CartPlugin : ISubmissionPlugin
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
                else if (action == "add-item")
                {
                    result = AddItem(site, controllerContext, submissionSetting);
                }
                else if (action == "remove-item")
                {
                    result = RemoveItem(site, controllerContext, submissionSetting);
                }
                else if (action == "update-quantity")
                {
                    result = UpdateQuantity(site, controllerContext, submissionSetting);
                }
                else if (action == "change-price")
                {
                    result = ChangePrice(site, controllerContext, submissionSetting);
                }
                else if (action == "apply-coupon")
                {
                    result = ApplyCoupon(site, controllerContext, submissionSetting);
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
            var api = site.Commerce();
            var query = api.ShoppingCarts.Query();
            var sessionId = controllerContext.HttpContext.Session.SessionID;
            var user = controllerContext.HttpContext.Membership().GetMembershipUser();
            if (user == null)
            {
                query = query.BySessionId(sessionId);
            }
            else
            {
                query = query.ByAccountId(user.UUID);
            }

            query.LoadWithBrands();
            query.LoadWithProductPrices();
            query.LoadWithProductImages();

            return query.FirstOrDefault() ?? new ShoppingCart
            {
                SessionId = sessionId
            };
        }

        private ShoppingCart AddItem(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            var httpContext = controllerContext.HttpContext;
            var request = httpContext.Request;

            var sessionId = httpContext.Session.SessionID;
            var member = httpContext.Membership().GetMembershipUser();
            var productPriceId = Convert.ToInt32(request.Form["productPriceId"]);
            var quantity = Convert.ToInt32(request.Form["quantity"]);

            if (!site.Commerce().ShoppingCarts.AddToCart(sessionId, member == null ? null : member.UUID, productPriceId, quantity))
                throw new Exception("Add to cart failed, please try again later.");

            return CartInfo(site, controllerContext, submissionSetting);
        }

        private ShoppingCart RemoveItem(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            var sessionId = controllerContext.HttpContext.Session.SessionID;
            var cart = site.Commerce().ShoppingCarts.BySessionId(sessionId).FirstOrDefault();
            if (cart != null) {
                var itemId = Convert.ToInt32(controllerContext.HttpContext.Request["itemId"]);
                site.Commerce().ShoppingCarts.RemoveCartItem(cart.Id, itemId); 
                return CartInfo(site, controllerContext, submissionSetting);
            }

            return null;
        }

        private ShoppingCart UpdateQuantity(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            var sessionId = controllerContext.HttpContext.Session.SessionID;
            var member = controllerContext.HttpContext.Membership().GetMembershipUser();
            var productPriceId = Convert.ToInt32(controllerContext.HttpContext.Request["productPriceId"]);
            var quantity = Convert.ToInt32(controllerContext.HttpContext.Request["quantity"]);
            site.Commerce().ShoppingCarts.UpdateCart(sessionId, member == null ? null : member.UUID, productPriceId, quantity);

            return CartInfo(site, controllerContext, submissionSetting);
        }

        private ShoppingCart ChangePrice(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            var request = controllerContext.HttpContext.Request;
            var itemId = Convert.ToInt32(request["itemId"]);
            var newProductPriceId = Convert.ToInt32(request["newProductPriceId"]);
            var sessionId = controllerContext.HttpContext.Session.SessionID;
            var member = controllerContext.HttpContext.Membership().GetMembershipUser();
            var cart = site.Commerce().ShoppingCarts.BySessionId(sessionId).FirstOrDefault();
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(x => x.Id == itemId);
                var quantity = item.Quantity;

                site.Commerce().ShoppingCarts.RemoveCartItem(cart.Id, itemId);
                site.Commerce().ShoppingCarts.AddToCart(sessionId, member == null ? null : member.UUID, newProductPriceId, quantity);

                return CartInfo(site, controllerContext, submissionSetting);
            }

            return null;
        }

        private ShoppingCart ApplyCoupon(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            var request = controllerContext.HttpContext.Request;
            var sessionId = controllerContext.HttpContext.Session.SessionID;
            var coupon = request["coupon"];
            var cart = site.Commerce().ShoppingCarts.BySessionId(sessionId).FirstOrDefault();
            if (cart != null)
            {
                if (!site.Commerce().ShoppingCarts.ApplyCoupon(cart.Id, coupon))
                {
                    throw new Exception("Invalid coupon code.");
                }
            }

            return CartInfo(site, controllerContext, submissionSetting);
        }
    }
}
