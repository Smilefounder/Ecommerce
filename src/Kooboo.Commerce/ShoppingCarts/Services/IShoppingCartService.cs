using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Customers;

namespace Kooboo.Commerce.ShoppingCarts.Services
{
    public interface IShoppingCartService
    {
        ShoppingCart GetByGuestId(Guid guestId);

        ShoppingCart GetByCustomer(int customerId);

        void Create(ShoppingCart shoppingCart);

        void Update(ShoppingCart shoppingCart);

        /// <summary>
        /// add to cart
        /// quantity should greater than 0.
        /// </summary>
        /// <param name="guestId">guest id</param>
        /// <param name="customerId">customer id</param>
        /// <param name="productPriceId">product price id</param>
        /// <param name="quantity">quantity</param>
        bool AddToCart(Guid? guestId, int? customerId, int productPriceId, int quantity);

        /// <summary>
        /// update cart
        /// if quantity <= 0 then remove the corresponding product else update the quantity in cart
        /// </summary>
        /// <param name="guestId">guest id</param>
        /// <param name="customerId">customer id</param>
        /// <param name="productPriceId">product price id</param>
        /// <param name="quantity">quantity</param>
        bool UpdateCart(Guid? guestId, int? customerId, int productPriceId, int quantity);

        bool FillCustomerByAccount(Guid guestId, int accountId);
    }
}