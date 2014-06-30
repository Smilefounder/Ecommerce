using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.ShoppingCarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.RestProvider.ShoppingCarts
{
    /// <summary>
    /// shopping cart api
    /// </summary>
    [Dependency(typeof(IShoppingCartAPI))]
    [Dependency(typeof(IShoppingCartQuery))]
    public class ShoppingCartAPI : RestApiQueryBase<ShoppingCart>, IShoppingCartAPI
    {
        public IShoppingCartQuery ById(int id)
        {
            QueryParameters.Add("id", id.ToString());
            return this;
        }

        /// <summary>
        /// add session id filter to query
        /// </summary>
        /// <param name="sessionId">session id</param>
        /// <returns>shopping cart query</returns>
        public IShoppingCartQuery BySessionId(string sessionId)
        {
            QueryParameters.Add("sessionId", sessionId);
            return this;
        }

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">account id</param>
        /// <returns>shopping cart query</returns>
        public IShoppingCartQuery ByAccountId(string accountId)
        {
            QueryParameters.Add("accountId", accountId);
            return this;
        }

        public int CreteCart()
        {
            return Post<int>(null);
        }

        public int CustomerCartId(string accountId)
        {
            QueryParameters.Add("accountId", accountId);
            return Get<int>("CustomerCartId");
        }

        public int SessionCartId(string sessionId)
        {
            QueryParameters.Add("sessionId", sessionId);
            return Get<int>("SessionCartId");
        }

        public bool ApplyCoupon(int cartId, string coupon)
        {
            QueryParameters.Add("cartId", cartId.ToString());
            QueryParameters.Add("coupon", coupon);
            return Post<bool>("ApplyCoupon");
        }

        public void ChangeShippingAddress(int cartId, Address address)
        {
            QueryParameters.Add("cartId", cartId.ToString());
            Post<bool>("ChangeShippingAddress", address);
        }

        public void ChangeBillingAddress(int cartId, Address address)
        {
            QueryParameters.Add("cartId", cartId.ToString());
            Post<bool>("ChangeBillingAddress", address);
        }

        public void MigrateCart(int customerId, string sessionId)
        {
            QueryParameters.Add("customerId", customerId.ToString());
            QueryParameters.Add("sessionId", sessionId);
            Post<bool>("MigrateCart");
        }

        public void ChangeItemQuantity(int cartId, int itemId, int newQuantity)
        {
            QueryParameters.Add("cartId", cartId.ToString());
            QueryParameters.Add("itemId", itemId.ToString());
            QueryParameters.Add("newQuantity", newQuantity.ToString());

            Post<bool>("ChangeItemQuantity");
        }

        public void ChangeShippingMethod(int cartId, int shippingMethodId)
        {
            QueryParameters.Add("cartId", cartId.ToString());
            QueryParameters.Add("shippingMethodId", shippingMethodId.ToString());

            Post<bool>("ChangeShippingMethod");
        }
        
        /// <summary>
        /// expire the shopping cart, so that user can create another new shopping cart by current session id
        /// </summary>
        /// <param name="shoppingCartId">shopping cart id</param>
        /// <returns>true if successfully, else false</returns>
        public void ExpireCart(int shoppingCartId)
        {
            QueryParameters.Add("shoppingCartId", shoppingCartId.ToString());
            Post<bool>("ExpireShppingCart");
        }

        /// <summary>
        /// create shopping cart query
        /// </summary>
        /// <returns>shopping cart query</returns>
        public IShoppingCartQuery Query()
        {
            return this;
        }

        /// <summary>
        /// create shopping cart data access
        /// </summary>
        /// <returns>shopping cart data access</returns>
        public IShoppingCartAccess Access()
        {
            return this;
        }


        public int AddItem(int cartId, int productPriceId, int quantity)
        {
            QueryParameters.Add("cartId", cartId.ToString());
            QueryParameters.Add("productPriceId", productPriceId.ToString());
            QueryParameters.Add("quantity", quantity.ToString());

            return Post<int>("AddItem");
        }

        public bool RemoveItem(int cartId, int itemId)
        {
            QueryParameters.Add("cartId", cartId.ToString());
            QueryParameters.Add("itemId", itemId.ToString());

            return Delete<bool>("RemoveItem");
        }
    }
}
