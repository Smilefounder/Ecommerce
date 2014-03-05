using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.ShoppingCarts.Services
{
    [Dependency(typeof(IShoppingCartService))]
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<ProductPrice> _productPriceRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<Customer> customerRepository, IRepository<ProductPrice> productPriceRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _customerRepository = customerRepository;
            _productPriceRepository = productPriceRepository;
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

        public ShoppingCart GetByCustomer(int customerId)
        {
            ShoppingCart shoppingCart = _shoppingCartRepository.Query()
                .Where(x => x.Customer.Id == customerId)
                .FirstOrDefault();

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.Customer = _customerRepository.Query(o => o.Id == customerId).FirstOrDefault();
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

        /// <summary>
        /// add to cart
        /// quantity should greater than 0.
        /// </summary>
        /// <param name="guestId">guest id</param>
        /// <param name="customerId">customer id</param>
        /// <param name="productPriceId">product price id</param>
        /// <param name="quantity">quantity</param>
        public bool AddToCart(Guid? guestId, int? customerId, int productPriceId, int quantity)
        {
            Require.That((guestId != null || customerId != null), "guestId, customerId");
            Require.That(quantity > 0, "quantity");

            var query = _shoppingCartRepository.Query();
            if (customerId.HasValue)
                query = query.Where(o => o.Customer.Id == customerId);
            else if (guestId.HasValue)
                query = query.Where(o => o.GuestId == guestId.Value);

            var cart = query.FirstOrDefault();
            if (cart == null)
            {
                Require.NotNull(guestId, "guestId");
                cart = new ShoppingCart();
                cart.GuestId = guestId.Value;
                cart.Customer = customerId.HasValue ? _customerRepository.Query(o => o.Id == customerId.Value).FirstOrDefault() : null;
                cart.Items = new List<ShoppingCartItem>();
                var productPrice = _productPriceRepository.Query(o => o.Id == productPriceId).First();
                var cartItem = new ShoppingCartItem(productPrice, quantity, cart);
                cart.Items.Add(cartItem);
                return _shoppingCartRepository.Insert(cart);
            }
            else
            {
                var cartItem = cart.Items.FirstOrDefault(o => o.ProductPrice.Id == productPriceId);
                if (cartItem == null)
                {
                    var productPrice = _productPriceRepository.Query(o => o.Id == productPriceId).First();
                    cartItem = new ShoppingCartItem(productPrice, quantity, cart);
                    cart.Items.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity += quantity;
                }
                return _shoppingCartRepository.Update(cart, k => new object[] { k.Id });
            }
        }

        /// <summary>
        /// update cart
        /// if quantity <= 0 then remove the corresponding product else update the quantity in cart
        /// </summary>
        /// <param name="guestId">guest id</param>
        /// <param name="customerId">customer id</param>
        /// <param name="productPriceId">product price id</param>
        /// <param name="quantity">quantity</param>
        public bool UpdateCart(Guid? guestId, int? customerId, int productPriceId, int quantity)
        {
            Require.That((guestId != null || customerId != null), "guestId, customerId");

            var query = _shoppingCartRepository.Query();
            if (customerId.HasValue)
                query = query.Where(o => o.Customer.Id == customerId);
            else if (guestId.HasValue)
                query = query.Where(o => o.GuestId == guestId.Value);

            var cart = query.FirstOrDefault();
            if (cart != null)
            {
                var cartItem = cart.Items.FirstOrDefault(o => o.ProductPrice.Id == productPriceId);
                if (cartItem != null)
                {
                    if (quantity > 0)
                    {
                        cartItem.Quantity = quantity;
                    }
                    else
                    {
                        cart.Items.Remove(cartItem);
                    }
                }
                return _shoppingCartRepository.Update(cart, k => new object[] { k.Id });
            }
            return AddToCart(guestId, customerId, productPriceId, quantity);
        }

        public bool FillCustomerByAccount(Guid guestId, int accountId)
        {
            Require.NotNull(guestId, "guestId");
            var cart = _shoppingCartRepository.Query(o => o.GuestId == guestId).FirstOrDefault();
            if(cart != null)
            {
                var customer = _customerRepository.Query(o => o.AccountId == accountId).FirstOrDefault();
                if(customer != null)
                {
                    cart.Customer = customer;
                    return _shoppingCartRepository.Update(cart, k => new object[] { k.Id });
                }
            }
            return false;
        }

        #endregion
    }
}