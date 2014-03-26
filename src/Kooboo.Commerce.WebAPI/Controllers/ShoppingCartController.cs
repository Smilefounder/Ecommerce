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
        /// <summary>
        /// build the commerce query filters from query string.
        /// </summary>
        /// <returns>commerce query</returns>
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
        /// <summary>
        /// add item to shopping cart
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        [HttpPost]
        public bool AddCartItem(int cartId, [FromBody]ShoppingCartItem item)
        {
            return Commerce().ShoppingCarts.AddCartItem(cartId, item);
        }

        /// <summary>
        /// update shopping cart item
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        [HttpPost]
        public bool UpdateCartItem(int cartId, [FromBody]ShoppingCartItem item)
        {
            return Commerce().ShoppingCarts.UpdateCartItem(cartId, item);
        }
        /// <summary>
        /// remove shopping cart item
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        [HttpDelete]
        public bool RemoveCartItem(int cartId, int cartItemId)
        {
            return Commerce().ShoppingCarts.RemoveCartItem(cartId, cartItemId);
        }

        /// <summary>
        /// add the specified product to current user's shopping cart
        /// add up the amount if the product already in the shopping item
        /// </summary>
        /// <param name="sessionId">current session id</param>
        /// <param name="accountId">current user's account id</param>
        /// <param name="productPriceId">specified product price</param>
        /// <param name="quantity">quantity</param>
        /// <returns>true if successfully, else false</returns>
        [HttpPost]
        public bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            return Commerce().ShoppingCarts.AddToCart(sessionId, accountId, productPriceId, quantity);
        }

        /// <summary>
        /// update the specified product's quantity
        /// update the amount if the product already in the shopping item
        /// </summary>
        /// <param name="sessionId">current session id</param>
        /// <param name="accountId">current user's account id</param>
        /// <param name="productPriceId">specified product price</param>
        /// <param name="quantity">quantity</param>
        /// <returns>true if successfully, else false</returns>
        [HttpPost]
        public bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            return Commerce().ShoppingCarts.UpdateCart(sessionId, accountId, productPriceId, quantity);
        }

        /// <summary>
        /// fill with customer info by current user's account
        /// </summary>
        /// <param name="sessionId">current session id</param>
        /// <param name="user">current user's info</param>
        /// <returns>true if successfully, else false</returns>
        [HttpPost]
        public bool FillCustomerByAccount(string sessionId, [FromBody]Kooboo.CMS.Membership.Models.MembershipUser user)
        {
            return Commerce().ShoppingCarts.FillCustomerByAccount(sessionId, user);
        }

        /// <summary>
        /// expire the shopping cart, so that user can create another new shopping cart by current session id
        /// </summary>
        /// <param name="shoppingCartId">shopping cart id</param>
        /// <returns>true if successfully, else false</returns>
        [HttpPost]
        public bool ExpireShppingCart(int shoppingCartId)
        {
            return Commerce().ShoppingCarts.ExpireShppingCart(shoppingCartId);
        }

    }
}
