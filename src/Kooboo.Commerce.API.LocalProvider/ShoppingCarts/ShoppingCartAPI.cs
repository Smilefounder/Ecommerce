using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.API.Locations;
using Kooboo.Commerce.API.Pricing;
using Kooboo.Commerce.API.ShoppingCarts;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Pricing;
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
    [Dependency(typeof(IShoppingCartAPI), ComponentLifeStyle.Transient)]
    public class ShoppingCartAPI : LocalCommerceQuery<ShoppingCart, Kooboo.Commerce.ShoppingCarts.ShoppingCart>, IShoppingCartAPI
    {
        private ICommerceDatabase _db;
        private IPriceAPI _priceApi;
        private IShoppingCartService _shoppingCartService;
        private ICustomerAPI _customerApi;
        private ICustomerService _customerService;
        private IPromotionService _promotionService;
        private IPromotionPolicyFactory _promotionPolicyFactory;
        private IMapper<ShoppingCartItem, Kooboo.Commerce.ShoppingCarts.ShoppingCartItem> _cartItemMapper;

        public ShoppingCartAPI(
            IHalWrapper halWrapper,
            ICommerceDatabase db,
            IPriceAPI priceApi,
            ICustomerAPI customerApi,
            IShoppingCartService shoppingCartService, 
            ICustomerService customerService,
            IPromotionService promotionService,
            IPromotionPolicyFactory promotionPolicyFactory,
            IMapper<ShoppingCart, Kooboo.Commerce.ShoppingCarts.ShoppingCart> mapper,
            IMapper<ShoppingCartItem, Kooboo.Commerce.ShoppingCarts.ShoppingCartItem> cartItemMapper)
            : base(halWrapper, mapper)
        {
            _db = db;
            _priceApi = priceApi;
            _customerApi = customerApi;
            _shoppingCartService = shoppingCartService;
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
            return _shoppingCartService.Query();
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
            _query = _query.Where(o => o.Customer.AccountId == accountId && !o.SessionId.StartsWith("EXPIRED_"));
            return this;
        }

        protected override ShoppingCart Map(Commerce.ShoppingCarts.ShoppingCart obj)
        {
            var cart = base.Map(obj);

            foreach (var item in cart.Items)
            {
                // TODO: hack for now
                item.ProductPriceId = item.ProductPrice.Id;
            }
            
            // calculate prices
            var prices = _priceApi.CartPrice(cart.Id);

            foreach (var item in prices.Items)
            {
                var cartItem = cart.Items.FirstOrDefault(x => x.Id == item.Id);
                cartItem.Subtotal = item.Subtotal.OriginalValue;
                cartItem.Discount = item.Subtotal.Discount;
            }

            cart.Subtotal = prices.Subtotal.FinalValue;
            cart.TotalDiscount = prices.Subtotal.Discount + cart.Items.Sum(x => x.Discount);
            cart.Total = prices.Total;

            return cart;
        }

        public bool ApplyCoupon(int cartId, string coupon)
        {
            using (var tx = _db.BeginTransaction())
            {
                if (_shoppingCartService.ApplyCoupon(cartId, coupon))
                {
                    tx.Commit();
                    return true;
                }

                return false;
            }
        }

        public bool ChangeShippingAddress(int cartId, Address address)
        {
            var cart = _shoppingCartService.Query().ById(cartId);
            var addr = GetOrCreateAddress(cart.Customer.Id, address);

            if (address.Id == 0)
            {
                address.Id = addr.Id;
            }

            using (var tx = _db.BeginTransaction())
            {
                cart.ShippingAddress = addr;
                tx.Commit();
            }

            return true;
        }

        public bool ChangeBillingAddress(int cartId, Address address)
        {
            var cart = _shoppingCartService.Query().ById(cartId);
            var addr = GetOrCreateAddress(cart.Customer.Id, address);

            if (address.Id == 0)
            {
                address.Id = addr.Id;
            }

            using (var tx = _db.BeginTransaction())
            {
                cart.BillingAddress = addr;
                tx.Commit();
            }

            return true;
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

        /// <summary>
        /// add item to shopping cart
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        public bool AddCartItem(int cartId, ShoppingCartItem item)
        {
            if (item != null)
            {
                return _shoppingCartService.AddCartItem(cartId, _cartItemMapper.MapFrom(item));
            }
            return false;
        }

        /// <summary>
        /// update shopping cart item
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        public bool UpdateCartItem(int cartId, ShoppingCartItem item)
        {
            if (item != null)
            {
                return _shoppingCartService.UpdateCartItem(cartId, _cartItemMapper.MapFrom(item));
            }
            return false;
        }

        /// <summary>
        /// remove shopping cart item
        /// </summary>
        /// <param name="cartId">cart id</param>
        /// <param name="item">shopping cart item</param>
        /// <returns>true if successfully, else false</returns>
        public bool RemoveCartItem(int cartId, int cartItemId)
        {
            return _shoppingCartService.RemoveCartItem(cartId, cartItemId);
        }

        /// <summary>
        /// add the specified product to current user's shopping cart
        /// add up the amount if the product already in the shopping item
        /// </summary>
        /// <param name="sessionId">current session id</param>
        /// <param name="accountId">current user's account id</param>
        /// <param name="productPriceId">specified product price</param>
        /// <param name="quantity">quantity</param>
        /// <returns>true if successfully, else false</returns>
        public bool AddToCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            int? customerId = null;
            if (!string.IsNullOrEmpty(accountId))
            {
                var customer = _customerService.Query().Where(o => o.AccountId == accountId).FirstOrDefault();

                if (customer != null)
                {
                    customerId = customer.Id;
                }
            }
            return _shoppingCartService.AddToCart(sessionId, customerId, productPriceId, quantity);
        }

        /// <summary>
        /// update the specified product's quantity
        /// update the amount if the product already in the shopping item
        /// </summary>
        /// <param name="sessionId">current session id</param>
        /// <param name="accountId">current user's account id</param>
        /// <param name="productPriceId">specified product price</param>
        /// <param name="quantity">quantity</param>
        /// <returns>true if successfully, else false</returns>
        public bool UpdateCart(string sessionId, string accountId, int productPriceId, int quantity)
        {
            int? customerId = null;
            if (!string.IsNullOrEmpty(accountId))
            {
                var customer = _customerService.Query().Where(o => o.AccountId == accountId).FirstOrDefault();

                if (customer != null)
                {
                    customerId = customer.Id;
                }
            }
            return _shoppingCartService.UpdateCart(sessionId, customerId, productPriceId, quantity);
        }

        /// <summary>
        /// fill with customer info by current user's account
        /// </summary>
        /// <param name="sessionId">current session id</param>
        /// <param name="user">current user's info</param>
        /// <returns>true if successfully, else false</returns>
        public bool FillCustomerByAccount(string sessionId, Kooboo.CMS.Membership.Models.MembershipUser user)
        {
            return _shoppingCartService.FillCustomerByAccount(sessionId, user);
        }

        /// <summary>
        /// expire the shopping cart, so that user can create another new shopping cart by current session id
        /// </summary>
        /// <param name="shoppingCartId">shopping cart id</param>
        /// <returns>true if successfully, else false</returns>
        public bool ExpireShppingCart(int shoppingCartId)
        {
            var shoppingCart = _shoppingCartService.Query().Where(o => o.Id == shoppingCartId).FirstOrDefault();
            if(shoppingCart != null)
            {
                return _shoppingCartService.ExpireShppingCart(shoppingCart);
            }
            return false;
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
