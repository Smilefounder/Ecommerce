using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Customers;
using Kooboo.Commerce.API.HAL;
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
        private IShoppingCartService _shoppingCartService;
        private ICustomerService _customerService;
        private IPromotionService _promotionService;
        private IPromotionPolicyFactory _promotionPolicyFactory;
        private IMapper<ShoppingCart, Kooboo.Commerce.ShoppingCarts.ShoppingCart> _mapper;
        private IMapper<ShoppingCartItem, Kooboo.Commerce.ShoppingCarts.ShoppingCartItem> _cartItemMapper;
        private IMapper<Customer, Kooboo.Commerce.Customers.Customer> _customerMapper;
        private bool _loadWithCutomer = false;
        private bool _loadWithBrands = false;
        private bool _loadWithProductPrices = false;
        private bool _loadWithProductImages = false;

        public ShoppingCartAPI(
            IHalWrapper halWrapper,
            ICommerceDatabase db,
            IShoppingCartService shoppingCartService, 
            ICustomerService customerService,
            IPromotionService promotionService,
            IPromotionPolicyFactory promotionPolicyFactory,
            IMapper<ShoppingCart, Kooboo.Commerce.ShoppingCarts.ShoppingCart> mapper,
            IMapper<ShoppingCartItem, Kooboo.Commerce.ShoppingCarts.ShoppingCartItem> cartItemMapper,
            IMapper<Customer, Kooboo.Commerce.Customers.Customer> customerMapper)
            : base(halWrapper)
        {
            _db = db;
            _shoppingCartService = shoppingCartService;
            _customerService = customerService;
            _promotionService = promotionService;
            _promotionPolicyFactory = promotionPolicyFactory;
            _mapper = mapper;
            _cartItemMapper = cartItemMapper;
            _customerMapper = customerMapper;
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
        /// map the entity to object
        /// </summary>
        /// <param name="obj">entity</param>
        /// <returns>object</returns>
        protected override ShoppingCart Map(Commerce.ShoppingCarts.ShoppingCart obj)
        {
            List<string> includeComplexPropertyNames = new List<string>();
            includeComplexPropertyNames.Add("Items");
            includeComplexPropertyNames.Add("Items.ProductPrice");
            includeComplexPropertyNames.Add("Items.ProductPrice.Product");
            includeComplexPropertyNames.Add("ShippingAddress");
            includeComplexPropertyNames.Add("BillingAddress");
            if (_loadWithCutomer)
            {
                includeComplexPropertyNames.Add("Customer");
            }
            if (_loadWithBrands)
            {
                includeComplexPropertyNames.Add("Items.ProductPrice.Product.Brand");
            }
            if (_loadWithProductPrices)
            {
                includeComplexPropertyNames.Add("Items.ProductPrice.Product.PriceList");
            }
            if (_loadWithProductImages)
            {
                includeComplexPropertyNames.Add("Items.ProductPrice.Product.Images");
            }

            var cart = _mapper.MapTo(obj, includeComplexPropertyNames.ToArray());

            foreach (var item in cart.Items)
            {
                item.ProductPriceId = item.ProductPrice.Id;
            }

            // Calculate promotion discounts
            var context = PricingContext.CreateFrom(obj);
            new PricingPipeline().Execute(context);

            foreach (var item in cart.Items)
            {
                var pricingItem = context.Items.FirstOrDefault(x => x.Id == item.Id);
                item.Subtotal = pricingItem.Subtotal.OriginalValue;
                item.Discount = pricingItem.Subtotal.Discount;
                item.Total = pricingItem.Subtotal.FinalValue;
            }

            cart.Subtotal = context.Subtotal.OriginalValue;
            cart.TotalDiscount = context.Items.Sum(x => x.Subtotal.Discount) + context.Subtotal.Discount;
            cart.Total = context.Total;

            cart.AppliedPromotions = context.AppliedPromotions.Select(p => new Promotions.Promotion
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToList();

            return cart;
        }

        /// <summary>
        /// this method will be called after query executed
        /// </summary>
        protected override void OnQueryExecuted()
        {
            base.OnQueryExecuted();
            _loadWithCutomer = false;
        }

        /// <summary>
        /// load shopping cart with customer info
        /// </summary>
        /// <returns>shopping cart query</returns>
        public IShoppingCartQuery LoadWithCustomer()
        {
            _loadWithCutomer = true;
            return this;
        }

        public IShoppingCartQuery LoadWithBrands()
        {
            _loadWithBrands = true;
            return this;
        }

        public IShoppingCartQuery LoadWithProductPrices()
        {
            _loadWithProductPrices = true;
            return this;
        }

        public IShoppingCartQuery LoadWithProductImages()
        {
            _loadWithProductImages = true;
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
            _query = _query.Where(o => o.SessionId == sessionId);
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
