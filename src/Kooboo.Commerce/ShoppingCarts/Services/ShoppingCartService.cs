using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.ShoppingCarts.Services
{
    [Dependency(typeof (IShoppingCartService))]
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        #region IShoppingCartService Members

        public ShoppingCart GetByGuestId(Guid guestId)
        {
            ShoppingCart shoppingCart = _shoppingCartRepository.Query()
                .Where(x => x.GuestId == guestId)
                .FirstOrDefault();

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.GuestId = guestId;
                Create(shoppingCart);
            }

            return shoppingCart;
        }

        public ShoppingCart GetByCustomer(Customer customer)
        {
            ShoppingCart shoppingCart = _shoppingCartRepository.Query()
                .Where(x => x.Customer.Id == customer.Id)
                .FirstOrDefault();

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.Customer = customer;
                Create(shoppingCart);
            }

            return shoppingCart;
        }

        public void Create(ShoppingCart shoppingCart)
        {
            _shoppingCartRepository.Insert(shoppingCart);
        }

        public void Update(ShoppingCart shoppingCart)
        {
            _shoppingCartRepository.Update(shoppingCart, k => new object[] { k.Id });
        }

        #endregion
    }
}