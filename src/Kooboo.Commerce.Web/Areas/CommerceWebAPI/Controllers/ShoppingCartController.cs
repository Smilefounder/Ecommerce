using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.ShoppingCarts;
using Kooboo.Commerce.API;
using Kooboo.Commerce.API.Locations;

namespace Kooboo.Commerce.Web.Areas.CommerceWebAPI.Controllers
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

            return BuildLoadWithFromQueryStrings(query, qs);
        }

        [HttpPost]
        public bool ApplyCoupon(int cartId, string coupon)
        {
            return Commerce().ShoppingCarts.ApplyCoupon(cartId, coupon);
        }

        [HttpPost]
        public int ChangeShippingAddress(int cartId, Address address)
        {
            Commerce().ShoppingCarts.ChangeShippingAddress(cartId, address);
            return address.Id;
        }

        [HttpPost]
        public int ChangeBillingAddress(int cartId, Address address)
        {
            Commerce().ShoppingCarts.ChangeBillingAddress(cartId, address);
            return address.Id;
        }

        [HttpPost]
        public int AddItem(int cartId, int productPriceId, int quantity)
        {
            return Commerce().ShoppingCarts.AddItem(cartId, productPriceId, quantity);
        }

        [HttpDelete]
        public bool RemoveItem(int cartId, int itemId)
        {
            return Commerce().ShoppingCarts.RemoveItem(cartId, itemId);
        }

        /// <summary>
        /// update shopping cart item
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        [HttpPost]
        public bool ChangeItemQuantity(int cartId, int itemId, int newQuantity)
        {
            Commerce().ShoppingCarts.ChangeItemQuantity(cartId, itemId, newQuantity);
            return true;
        }

        /// <summary>
        /// expire the shopping cart, so that user can create another new shopping cart by current session id
        /// </summary>
        /// <param name="shoppingCartId">shopping cart id</param>
        /// <returns>true if successfully, else false</returns>
        [HttpPost]
        public bool ExpireShppingCart(int shoppingCartId)
        {
            Commerce().ShoppingCarts.ExpireCart(shoppingCartId);
            return true;
        }

        [HttpPost]
        public bool MigrateCart(int customerId, string sessionId)
        {
            Commerce().ShoppingCarts.MigrateCart(customerId, sessionId);
            return true;
        }
    }
}
