using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.Pricing;
using Kooboo.Commerce.API.ShoppingCarts;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Promotions;
using Kooboo.Commerce.Promotions.Services;
using Kooboo.Commerce.ShoppingCarts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.ShoppingCarts
{
    /// <summary>
    /// shopping cart api
    /// </summary>
    [Dependency(typeof(IShoppingCartAPI))]
    [Dependency(typeof(IShoppingCartQuery))]
    public class ShoppingCartAPI : LocalCommerceQuery<ShoppingCart, Kooboo.Commerce.ShoppingCarts.ShoppingCart>, IShoppingCartAPI
    {
        private ICommerceDatabase _db;
        private IProductService _productService;
        private IPriceAPI _priceApi;
        private IShoppingCartService _cartService;
        private ICustomerAPI _customerApi;
        private ICustomerService _customerService;
        private IPromotionService _promotionService;
        private IPromotionPolicyProvider _promotionPolicyFactory;
        private IMapper<ShoppingCartItem, Kooboo.Commerce.ShoppingCarts.ShoppingCartItem> _cartItemMapper;

        public ShoppingCartAPI(
            ICommerceDatabase db,
            IProductService productService,
            IPriceAPI priceApi,
            ICustomerAPI customerApi,
            IShoppingCartService shoppingCartService,
            ICustomerService customerService,
            IPromotionService promotionService,
            IPromotionPolicyProvider promotionPolicyFactory,
            IMapper<ShoppingCart, Kooboo.Commerce.ShoppingCarts.ShoppingCart> mapper,
            IMapper<ShoppingCartItem, Kooboo.Commerce.ShoppingCarts.ShoppingCartItem> cartItemMapper)
            : base(mapper)
        {
            _db = db;
            _priceApi = priceApi;
            _productService = productService;
            _customerApi = customerApi;
            _cartService = shoppingCartService;
            _customerService = customerService;
            _promotionService = promotionService;
            _promotionPolicyFactory = promotionPolicyFactory;
            _cartItemMapper = cartItemMapper;

            Include(c => c.Items);
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Commerce.ShoppingCarts.ShoppingCart> CreateQuery()
        {
            return _cartService.Query();
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Commerce.ShoppingCarts.ShoppingCart> OrderByDefault(IQueryable<Commerce.ShoppingCarts.ShoppingCart> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        public IShoppingCartQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add session id filter to query
        /// </summary>
        /// <param name="sessionId">session id</param>
        /// <returns>shopping cart query</returns>
        public IShoppingCartQuery BySessionId(string sessionId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.SessionId == sessionId && o.Customer == null);
            return this;
        }

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">account id</param>
        /// <returns>shopping cart query</returns>
        public IShoppingCartQuery ByAccountId(string accountId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Customer.AccountId == accountId);
            return this;
        }

        protected override ShoppingCart Map(Commerce.ShoppingCarts.ShoppingCart obj)
        {
            Include(o => o.Items);
            Include(o => o.Items.Select(i => i.ProductPrice));
            var cart = base.Map(obj);
            // calculate prices
            var prices = _priceApi.CartPrice(cart.Id);

            foreach (var item in prices.Items)
            {
                var cartItem = cart.Items.FirstOrDefault(x => x.Id == item.Id);
                cartItem.Subtotal = item.Subtotal.OriginalValue;
                cartItem.Discount = item.Subtotal.Discount;
                cartItem.Total = item.Subtotal.FinalValue;
            }

            cart.Subtotal = prices.Subtotal.FinalValue;
            cart.TotalDiscount = prices.Subtotal.Discount + cart.Items.Sum(x => x.Discount);
            cart.Total = prices.Total;

            return cart;
        }

        public int CustomerCartId(string accountId)
        {
            var cart = _cartService.GetByAccountId(accountId);
            if (cart == null)
            {
                var customer = _customerService.GetByAccountId(accountId);
                cart = Kooboo.Commerce.ShoppingCarts.ShoppingCart.Create(customer);
                _cartService.Create(cart);
            }

            return cart.Id;
        }

        public int SessionCartId(string sessionId)
        {
            var cart = _cartService.GetBySessionId(sessionId);
            if (cart == null)
            {
                cart = Kooboo.Commerce.ShoppingCarts.ShoppingCart.Create(sessionId);
                _cartService.Create(cart);
            }

            return cart.Id;
        }

        public bool ApplyCoupon(int cartId, string coupon)
        {
            return _db.WithTransaction(() =>
            {
                var cart = _cartService.GetById(cartId);
                return _cartService.ApplyCoupon(cart, coupon);
            });
        }

        public int AddItem(int cartId, int productPriceId, int quantity)
        {
            var cart = _cartService.GetById(cartId);
            var productPrice = _productService.GetProductPriceById(productPriceId);

            return _db.WithTransaction(() =>
            {
                return _cartService.AddItem(cart, productPrice.Product, productPrice, quantity).Id;
            });
        }

        public bool RemoveItem(int cartId, int itemId)
        {
            var cart = _cartService.GetById(cartId);
            return _db.WithTransaction(() =>
            {
                return _cartService.RemoveItem(cart, itemId);
            });
        }

        public void ChangeShippingAddress(int cartId, Address address)
        {
            var cart = _cartService.GetById(cartId);

            _db.WithTransaction(() =>
            {
                var addr = GetOrCreateAddress(cart.Customer.Id, address);
                if (address.Id == 0)
                {
                    address.Id = addr.Id;
                }

                _cartService.ChangeShippingAddress(cart, addr);
            });
        }

        public void ChangeBillingAddress(int cartId, Address address)
        {
            var cart = _cartService.GetById(cartId);

            _db.WithTransaction(() =>
            {
                var addr = GetOrCreateAddress(cart.Customer.Id, address);
                if (address.Id == 0)
                {
                    address.Id = addr.Id;
                }

                _cartService.ChangeBillingAddress(cart, addr);
            });
        }

        private Kooboo.Commerce.Locations.Address GetOrCreateAddress(int customerId, Address address)
        {
            Kooboo.Commerce.Locations.Address addr = null;

            if (address.Id > 0)
            {
                addr = _customerService.QueryAddress().ById(address.Id);
            }
            else
            {
                _customerApi.AddAddress(customerId, address);
                addr = _customerService.QueryAddress().ById(address.Id);
            }

            return addr;
        }

        public void MigrateCart(int customerId, string session)
        {
            var sessionCart = _cartService.GetBySessionId(session);
            if (sessionCart == null)
            {
                return;
            }

            var customerCart = _cartService.GetByCustomer(customerId);
            if (customerCart == null)
            {
                var customer = _customerService.GetById(customerId);
                customerCart = Kooboo.Commerce.ShoppingCarts.ShoppingCart.Create(customer, session);
                _cartService.Create(customerCart);
            }

            _cartService.MigrateCart(sessionCart, customerCart);
        }

        public void ChangeItemQuantity(int cartId, int itemId, int newQuantity)
        {
            var cart = _cartService.GetById(cartId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _db.WithTransaction(() =>
                {
                    _cartService.ChangeItemQuantity(cart, item, newQuantity);
                });
            }
        }

        /// <summary>
        /// expire the shopping cart, so that user can create another new shopping cart by current session id
        /// </summary>
        /// <param name="cartId">shopping cart id</param>
        /// <returns>true if successfully, else false</returns>
        public void ExpireCart(int cartId)
        {
            var shoppingCart = _cartService.Query().Where(o => o.Id == cartId).FirstOrDefault();
            if (shoppingCart != null)
            {
                _cartService.ExpireCart(shoppingCart);
            }
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
