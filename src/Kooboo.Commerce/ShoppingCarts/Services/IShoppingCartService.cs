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

        ShoppingCart GetByCustomer(Customer customer);

        void Update(ShoppingCart shoppingCart);
    }
}