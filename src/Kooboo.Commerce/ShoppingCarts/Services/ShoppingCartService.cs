using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Customers.Services;
using Kooboo.CMS.Membership.Models;

namespace Kooboo.Commerce.ShoppingCarts.Services
{
    [Dependency(typeof(IShoppingCartService))]
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;
        private readonly ICustomerService _customerService;
        private readonly IRepository<ProductPrice> _productPriceRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<ShoppingCartItem> shoppingCartItemRepository, ICustomerService customerService, IRepository<ProductPrice> productPriceRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _customerService = customerService;
            _productPriceRepository = productPriceRepository;
        }

        #region IShoppingCartService Members

        public IQueryable<ShoppingCart> Query()
        {
            return _shoppingCartRepository.Query();
        }

        public IQueryable<ShoppingCartItem> ShoppingCartItemQuery()
        {
            return _shoppingCartItemRepository.Query();
        }


        //public ShoppingCart GetBySessionId(string sessionId)
        //{
        //    ShoppingCart shoppingCart = _shoppingCartRepository.Query()
        //        .Where(x => x.SessionId == sessionId)
        //        .FirstOrDefault();

        //    if (shoppingCart == null)
        //    {
        //        shoppingCart = new ShoppingCart();
        //        shoppingCart.SessionId = sessionId;
        //        Create(shoppingCart);
        //    }

        //    return shoppingCart;
        //}

        //public ShoppingCart GetByCustomer(int customerId)
        //{
        //    ShoppingCart shoppingCart = _shoppingCartRepository.Query()
        //        .Where(x => x.Customer.Id == customerId)
        //        .FirstOrDefault();

        //    if (shoppingCart == null)
        //    {
        //        shoppingCart = new ShoppingCart();
        //        shoppingCart.Customer = _customerService.GetById(customerId);
        //        Create(shoppingCart);
        //    }

        //    return shoppingCart;
        //}

        public bool Create(ShoppingCart shoppingCart)
        {
            return _shoppingCartRepository.Insert(shoppingCart);
        }

        public bool Update(ShoppingCart shoppingCart)
        {
            return _shoppingCartRepository.Update(shoppingCart, k => new object[] { k.Id });
        }

        public bool Save(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Id > 0)
            {
                bool exists = _shoppingCartRepository.Query(o => o.Id == shoppingCart.Id).Any();
                if (exists)
                    return Update(shoppingCart);
                else
                    return Create(shoppingCart);
            }
            else
            {
                return Create(shoppingCart);
            }
        }

        public bool Delete(ShoppingCart shoppingCart)
        {
            return _shoppingCartRepository.Delete(shoppingCart);
        }

        /// <summary>
        /// add to cart
        /// quantity should greater than 0.
        /// </summary>
        /// <param name="sessionId">session id</param>
        /// <param name="customerId">customer id</param>
        /// <param name="productPriceId">product price id</param>
        /// <param name="quantity">quantity</param>
        public bool AddToCart(string sessionId, int? customerId, int productPriceId, int quantity)
        {
            Require.That((sessionId != null || customerId != null), "sessionId, customerId");
            Require.That(quantity > 0, "quantity");

            var query = _shoppingCartRepository.Query();
            // always get cart from session id
            if (!string.IsNullOrEmpty(sessionId))
                query = query.Where(o => o.SessionId == sessionId);

            var cart = query.FirstOrDefault();
            if (cart == null)
            {
                Require.NotNull(sessionId, "sessionId");
                cart = new ShoppingCart();
                cart.SessionId = sessionId;
                cart.Customer = customerId.HasValue ? _customerService.GetById(customerId.Value) : null;
                cart.Items = new List<ShoppingCartItem>();
                var productPrice = _productPriceRepository.Query(o => o.Id == productPriceId).First();
                var cartItem = new ShoppingCartItem(productPrice, quantity, cart);
                cart.Items.Add(cartItem);
                return _shoppingCartRepository.Insert(cart);
            }
            else
            {
                if (cart.Customer == null && customerId.HasValue)
                    cart.Customer = _customerService.GetById(customerId.Value);
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
        /// <param name="sessionId">session id</param>
        /// <param name="customerId">customer id</param>
        /// <param name="productPriceId">product price id</param>
        /// <param name="quantity">quantity</param>
        public bool UpdateCart(string sessionId, int? customerId, int productPriceId, int quantity)
        {
            Require.That((sessionId != null || customerId != null), "sessionId, customerId");

            var query = _shoppingCartRepository.Query();
            if (customerId.HasValue)
                query = query.Where(o => o.Customer.Id == customerId);
            else if (!string.IsNullOrEmpty(sessionId))
                query = query.Where(o => o.SessionId == sessionId);

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
            return AddToCart(sessionId, customerId, productPriceId, quantity);
        }

        public bool FillCustomerByAccount(string sessionId, MembershipUser user)
        {
            Require.NotNull(sessionId, "sessionId");
            var cart = _shoppingCartRepository.Query(o => o.SessionId == sessionId).FirstOrDefault();
            if(cart != null)
            {
                var customer = _customerService.Query().Where(o => o.AccountId == user.UUID).FirstOrDefault();
                if(customer == null)
                {
                    customer = _customerService.CreateByAccount(user);
                }
                if(customer != null)
                {
                    cart.Customer = customer;
                    return _shoppingCartRepository.Update(cart, k => new object[] { k.Id });
                }
            }
            return false;
        }

        public bool ExpireShppingCart(ShoppingCart shoppingCart)
        {
            if (shoppingCart != null)
            {
                shoppingCart.SessionId += "_" + DateTime.UtcNow.Ticks.ToString();
                return _shoppingCartRepository.Update(shoppingCart, k => new object[] { k.Id });
            }
            return false;
        }

        #endregion


        public void AddCartItem(int cartId, ShoppingCartItem item)
        {
            var cart = _shoppingCartRepository.Query(o => o.Id == cartId).FirstOrDefault();
            if(cart != null)
            {
                item.ShoppingCart = cart;
                _shoppingCartItemRepository.Insert(item);
            }
        }

        public void UpdateCartItem(int cartId, ShoppingCartItem item)
        {
            _shoppingCartItemRepository.Update(item, k => new object[] { k.Id });
        }

        public void RemoveCartItem(int cartId, int cartItemId)
        {
            var cartItem = _shoppingCartItemRepository.Query(o => o.Id == cartItemId && o.ShoppingCart.Id == cartId).FirstOrDefault();
            if (cartItem != null)
            {
                _shoppingCartItemRepository.Delete(cartItem);
            }
        }
    }
}