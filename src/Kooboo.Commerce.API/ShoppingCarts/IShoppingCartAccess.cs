using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.ShoppingCarts
{
    /// <summary>
    /// shopping cart access
    /// </summary>
    public interface IShoppingCartAccess
    {
        bool ApplyCoupon(int cartId, string coupon);

        bool ChangeShippingAddress(int cartId, Address address);

        bool ChangeBillingAddress(int cartId, Address address);

        /// <summary>
        /// add item to shopping cart
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        bool AddCartItem(int cartId, ShoppingCartItem item);
        /// <summary>
        /// update shopping cart item
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        bool UpdateCartItem(int cartId, ShoppingCartItem item);
        /// <summary>
        /// remove shopping cart item
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        bool RemoveCartItem(int cartId, int cartItemId);
        /// <summary>
        /// add the specified product to current user's shopping cart
        /// add up the amount if the product already in the shopping item
        /// </summary>
        /// <param name="sessionId">current session id</param>
        /// <param name="accountId">current user's account id</param>
        /// <param name="productPriceId">specified product price</param>
        /// <param name="quantity">quantity</param>
        /// <returns>true if successfully, else false</returns>
        bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity);
        /// <summary>
        /// update the specified product's quantity
        /// update the amount if the product already in the shopping item
        /// </summary>
        /// <param name="sessionId">current session id</param>
        /// <param name="accountId">current user's account id</param>
        /// <param name="productPriceId">specified product price</param>
        /// <param name="quantity">quantity</param>
        /// <returns>true if successfully, else false</returns>
        bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity);
        /// <summary>
        /// fill with customer info by current user's account
        /// </summary>
        /// <param name="sessionId">current session id</param>
        /// <param name="user">current user's info</param>
        /// <returns>true if successfully, else false</returns>
        bool FillCustomerByAccount(string sessionId, MembershipUser user);
        /// <summary>
        /// expire the shopping cart, so that user can create another new shopping cart by current session id
        /// </summary>
        /// <param name="shoppingCartId">shopping cart id</param>
        /// <returns>true if successfully, else false</returns>
        bool ExpireShppingCart(int shoppingCartId);
    }
}
