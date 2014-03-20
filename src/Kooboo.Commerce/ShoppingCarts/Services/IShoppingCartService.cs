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
        IQueryable<ShoppingCart> Query();

        IQueryable<ShoppingCartItem> ShoppingCartItemQuery();

        bool Create(ShoppingCart shoppingCart);

        bool Update(ShoppingCart shoppingCart);
        bool Save(ShoppingCart shoppingCart);

        bool Delete(ShoppingCart shoppingCart);

        void AddCartItem(int cartId, ShoppingCartItem item);
        void UpdateCartItem(int cartId, ShoppingCartItem item);

        void RemoveCartItem(int cartId, int cartItemId);

        bool AddToCart(string sessionId, int? customerId, int productPriceId, int quantity);

        bool UpdateCart(string sessionId, int? customerId, int productPriceId, int quantity);

        bool FillCustomerByAccount(string sessionId, MembershipUser user);

        bool ExpireShppingCart(ShoppingCart shoppingCart);
    }
}