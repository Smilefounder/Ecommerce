using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Customers;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.ShoppingCarts.Services
{
    public interface IShoppingCartService
    {
        ShoppingCart GetBySessionId(string sessionId);

        ShoppingCart GetByCustomer(int customerId);

        void Create(ShoppingCart shoppingCart);

        void Update(ShoppingCart shoppingCart);

        /// <summary>
        /// add to cart
        /// quantity should greater than 0.
        /// </summary>
        /// <param name="sessionId">session id</param>
        /// <param name="customerId">customer id</param>
        /// <param name="productPriceId">product price id</param>
        /// <param name="quantity">quantity</param>
        bool AddToCart(string sessionId, int? customerId, int productPriceId, int quantity);

        /// <summary>
        /// update cart
        /// if quantity <= 0 then remove the corresponding product else update the quantity in cart
        /// </summary>
        /// <param name="sessionId">session id</param>
        /// <param name="customerId">customer id</param>
        /// <param name="productPriceId">product price id</param>
        /// <param name="quantity">quantity</param>
        bool UpdateCart(string sessionId, int? customerId, int productPriceId, int quantity);

        bool FillCustomerByAccount(string sessionId, MembershipUser user);
    }
}