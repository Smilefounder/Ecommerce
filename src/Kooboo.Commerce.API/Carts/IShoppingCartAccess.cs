using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Carts
{
    /// <summary>
    /// shopping cart access
    /// </summary>
    public interface IShoppingCartAccess
    {
        int CustomerCartId(string accountId);

        int SessionCartId(string sessionId);

        /// <summary>
        /// Apply coupon to the shopping cart. 
        /// If the visitor is an authenticated customer, pass his email, else pass the sessionId.
        /// </summary>
        bool ApplyCoupon(int cartId, string coupon);

        void ChangeShippingAddress(int cartId, Address address);

        void ChangeBillingAddress(int cartId, Address address);

        void ChangeShippingMethod(int cartId, int shippingMethodId);

        void MigrateCart(int customerId, string sessionId);

        /// <summary>
        /// Add a product to the shopping cart.
        /// </summary>
        /// <param name="cartId">The cart id.</param>
        /// <param name="productPriceId">The product price id.</param>
        /// <param name="quantity">The quantity.</param>
        /// <returns>The created cart item id.</returns>
        int AddItem(int cartId, int productPriceId, int quantity);

        /// <summary>
        /// Remove the specified cart item from the shopping cart.
        /// </summary>
        /// <param name="cartId">The cart id.</param>
        /// <param name="itemId">The item id.</param>
        /// <returns>True if the item is in the cart, else false.</returns>
        bool RemoveItem(int cartId, int itemId);

        /// <summary>
        /// Change the quantity of the specified item.
        /// </summary>
        /// <param name="cartId">The cart id.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="newQuantity">New quantity.</param>
        void ChangeItemQuantity(int cartId, int itemId, int newQuantity);

        /// <summary>
        /// expire the shopping cart, so that user can create another new shopping cart by current session id
        /// </summary>
        /// <param name="cartId">shopping cart id</param>
        /// <returns>true if successfully, else false</returns>
        void ExpireCart(int cartId);
    }
}
