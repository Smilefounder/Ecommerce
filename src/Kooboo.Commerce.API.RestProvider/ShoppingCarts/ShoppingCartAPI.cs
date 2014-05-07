using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
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
    [Dependency(typeof(IShoppingCartAPI), ComponentLifeStyle.Transient)]
    public class ShoppingCartAPI : RestApiQueryBase<ShoppingCart>, IShoppingCartAPI
    {
        /// <summary>
        /// load shopping cart with customer info
        /// </summary>
        /// <returns>shopping cart query</returns>
        public IShoppingCartQuery LoadWithCustomer()
        {
            QueryParameters.Add("LoadWithCustomer", "true");
            return this;
        }

        public IShoppingCartQuery LoadWithBrands()
        {
            QueryParameters.Add("LoadWithBrands", "true");
            return this;
        }

        public IShoppingCartQuery LoadWithProductPrices()
        {
            QueryParameters.Add("LoadWithProductPrices", "true");
            return this;
        }

        public IShoppingCartQuery LoadWithProductImages()
        {
            QueryParameters.Add("LoadWithProductImages", "true");
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

        /// <summary>
        /// add item to shopping cart
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        public bool AddCartItem(int cartId, ShoppingCartItem item)
        {
            if (item != null)
            {
                QueryParameters.Add("cartId", cartId.ToString());
                return Post<bool>("AddCartItem", item);
            }
            return false;
        }

        /// <summary>
        /// update shopping cart item
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        public bool UpdateCartItem(int cartId, ShoppingCartItem item)
        {
            if (item != null)
            {
                QueryParameters.Add("cartId", cartId.ToString());
                return Post<bool>("UpdateCartItem", item);
            }
            return false;
        }

        /// <summary>
        /// remove shopping cart item
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        public bool RemoveCartItem(int cartId, int cartItemId)
        {
            QueryParameters.Add("cartId", cartId.ToString());
            QueryParameters.Add("cartItemId", cartItemId.ToString());
            return Delete<bool>("RemoveCartItem");
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
        public bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            QueryParameters.Add("sessionId", sessionId);
            QueryParameters.Add("accountId", accountId);
            QueryParameters.Add("productPriceId", productPriceId.ToString());
            QueryParameters.Add("quantity", quantity.ToString());
            return Post<bool>("AddToCart");
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
        public bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            QueryParameters.Add("sessionId", sessionId);
            QueryParameters.Add("accountId", accountId);
            QueryParameters.Add("productPriceId", productPriceId.ToString());
            QueryParameters.Add("quantity", quantity.ToString());
            return Post<bool>("UpdateCart");
        }

        /// <summary>
        /// fill with customer info by current user's account
        /// </summary>
        /// <param name="sessionId">current session id</param>
        /// <param name="user">current user's info</param>
        /// <returns>true if successfully, else false</returns>
        public bool FillCustomerByAccount(string sessionId, Kooboo.CMS.Membership.Models.MembershipUser user)
        {
            QueryParameters.Add("sessionId", sessionId);
            return Post<bool>("FillCustomerByAccount", user);
        }


        /// <summary>
        /// expire the shopping cart, so that user can create another new shopping cart by current session id
        /// </summary>
        /// <param name="shoppingCartId">shopping cart id</param>
        /// <returns>true if successfully, else false</returns>
        public bool ExpireShppingCart(int shoppingCartId)
        {
            QueryParameters.Add("shoppingCartId", shoppingCartId.ToString());
            return Post<bool>("ExpireShppingCart");
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
    }
}
