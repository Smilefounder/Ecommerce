using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.ShoppingCarts;
using Kooboo.Commerce.API;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class ShoppingCartController : CommerceAPIControllerQueryBase<ShoppingCart>
    {
        protected override ICommerceQuery<ShoppingCart> BuildQueryFromQueryStrings()
        {
            var qs = Request.RequestUri.ParseQueryString();
            var query = Commerce().ShoppingCarts.Query();
            if (!string.IsNullOrEmpty(qs["sessionId"]))
                query = query.BySessionId(qs["sessionId"]);
            if (!string.IsNullOrEmpty(qs["accountId"]))
                query = query.ByAccountId(qs["accountId"]);


            if (qs["LoadWithCustomer"] == "true")
                query = query.LoadWithCustomer();

            return query;
        }
        [HttpPost]
        public bool AddCartItem(int cartId, [FromBody]ShoppingCartItem item)
        {
            return Commerce().ShoppingCarts.AddCartItem(cartId, item);
        }

        [HttpPost]
        public bool UpdateCartItem(int cartId, [FromBody]ShoppingCartItem item)
        {
            return Commerce().ShoppingCarts.UpdateCartItem(cartId, item);
        }
        [HttpDelete]
        public bool RemoveCartItem(int cartId, int cartItemId)
        {
            return Commerce().ShoppingCarts.RemoveCartItem(cartId, cartItemId);
        }

        [HttpPost]
        public bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            return Commerce().ShoppingCarts.AddToCart(sessionId, accountId, productPriceId, quantity);
        }

        [HttpPost]
        public bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            return Commerce().ShoppingCarts.UpdateCart(sessionId, accountId, productPriceId, quantity);
        }

        [HttpPost]
        public bool FillCustomerByAccount(string sessionId, [FromBody]Kooboo.CMS.Membership.Models.MembershipUser user)
        {
            return Commerce().ShoppingCarts.FillCustomerByAccount(sessionId, user);
        }


        [HttpPost]
        public bool ExpireShppingCart(int shoppingCartId)
        {
            return Commerce().ShoppingCarts.ExpireShppingCart(shoppingCartId);
        }

    }
}
