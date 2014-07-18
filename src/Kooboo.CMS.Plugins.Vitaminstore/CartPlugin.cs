using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.Commerce.API.Carts;
using Kooboo.Commerce.API.Products;
using Kooboo.CMS.Membership.Models;

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
                else if (action == "mini-cartinfo")
                {
                    result = MiniCartInfo(site, controllerContext);
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
            var cartId = controllerContext.HttpContext.CurrentCartId();

            query = query.ById(cartId);

            query.Include("Items.ProductPrice");
            query.Include("Items.ProductPrice.Product");
            query.Include("Items.ProductPrice.Product.Brand");
            query.Include("Items.ProductPrice.Product.PriceList");
            query.Include("Items.ProductPrice.Product.Images");

            return query.FirstOrDefault() ?? new ShoppingCart
            {
                SessionId = controllerContext.HttpContext.CurrentSessionId()
            };
        }

        private object MiniCartInfo(Site site, ControllerContext controllerContext)
        {
            var api = site.Commerce();

            var cart = GetShoppingCart(site, controllerContext);
            if (cart == null)
            {
                return null;
            }

            var count = cart.Items.Sum(x => x.Quantity);

            return new
            {
                TotalQuantity = count,
                Subtotal = cart.Subtotal,
                Total = cart.Total
            };
        }

        private ShoppingCart GetShoppingCart(Site site, ControllerContext controllerContext)
        {
            var cartId = controllerContext.HttpContext.CurrentCartId();
            return site.Commerce().ShoppingCarts.ById(cartId).FirstOrDefault();
        }
    }
}
