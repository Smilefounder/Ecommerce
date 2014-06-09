using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.API.Orders;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.ShoppingCarts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.API.LocalProvider.Orders
{
    /// <summary>
    /// order api
    /// </summary>
    [Dependency(typeof(IOrderAPI), ComponentLifeStyle.Transient)]
    [Dependency(typeof(IOrderQuery), ComponentLifeStyle.Transient)]
    public class OrderAPI : LocalCommerceQueryAccess<Order, Kooboo.Commerce.Orders.Order>, IOrderAPI
    {
        private IOrderService _orderService;
        private IShoppingCartService _shoppingCartService;
        private ICustomerService _customerService;

        public OrderAPI(IHalWrapper halWrapper, IOrderService orderService, IShoppingCartService shoppingCartService, ICustomerService customerService,
            IMapper<Order, Kooboo.Commerce.Orders.Order> mapper)
            : base(halWrapper, mapper)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
            _customerService = customerService;
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Commerce.Orders.Order> CreateQuery()
        {
            return _orderService.Query();
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Commerce.Orders.Order> OrderByDefault(IQueryable<Commerce.Orders.Order> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        /// <summary>
        /// this method will be called after query executed
        /// </summary>
        protected override void OnQueryExecuted()
        {
            base.OnQueryExecuted();
        }

        public Order CreateFromShoppingCart(int cartId, MembershipUser user, bool deleteShoppingCart)
        {
            var cart = _shoppingCartService.Query().ById(cartId);
            var order =_orderService.CreateFromCart(cart, user, deleteShoppingCart);
            return _mapper.MapTo(order);
        }

        /// <summary>
        /// create object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public override bool Create(Order obj)
        {
            if(obj != null)
            {
                return _orderService.Create(_mapper.MapFrom(obj));
            }
            return false;
        }

        /// <summary>
        /// update object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public override bool Update(Order obj)
        {
            if (obj != null)
            {
                return _orderService.Update(_mapper.MapFrom(obj));
            }
            return false;
        }

        /// <summary>
        /// create/update object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public override bool Save(Order obj)
        {
            if (obj != null)
            {
                return _orderService.Save(_mapper.MapFrom(obj));
            }
            return false;
        }

        /// <summary>
        /// delete object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public override bool Delete(Order obj)
        {
            if (obj != null)
            {
                return _orderService.Delete(_mapper.MapFrom(obj));
            }
            return false;
        }

        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">order id</param>
        /// <returns>order query</returns>
        public IOrderQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add customer id filter to query
        /// </summary>
        /// <param name="customerId">customer id</param>
        /// <returns>order query</returns>
        public IOrderQuery ByCustomerId(int customerId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.CustomerId == customerId);
            return this;
        }

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">account id</param>
        /// <returns>order query</returns>
        public IOrderQuery ByAccountId(string accountId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Customer.AccountId == accountId);
            return this;
        }

        /// <summary>
        /// add create date filter to query
        /// </summary>
        /// <param name="from">from date filter</param>
        /// <param name="to">to date filter</param>
        /// <returns>order query</returns>
        public IOrderQuery ByCreateDate(DateTime? from, DateTime? to)
        {
            EnsureQuery();
            if (from.HasValue)
                _query = _query.Where(o => o.CreatedAtUtc >= from.Value);
            if (to.HasValue)
                _query = _query.Where(o => o.CreatedAtUtc <= to.Value);
            return this;
        }

        /// <summary>
        /// add order status filter to query
        /// </summary>
        /// <param name="status">order status</param>
        /// <returns>order query</returns>
        public IOrderQuery ByOrderStatus(OrderStatus status)
        {
            EnsureQuery();
            _query = _query.Where(o => (int)o.OrderStatus == (int)status);
            return this;
        }

        /// <summary>
        /// add is completed filter to query
        /// </summary>
        /// <param name="isCompleted">order is completed</param>
        /// <returns>order query</returns>
        public IOrderQuery IsCompleted(bool isCompleted)
        {
            EnsureQuery();
            _query = _query.Where(o => o.IsCompleted == isCompleted);
            return this;
        }

        /// <summary>
        /// add coupon filter to query
        /// </summary>
        /// <param name="coupon">order coupon</param>
        /// <returns>order query</returns>
        public IOrderQuery ByCoupon(string coupon)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Coupon == coupon);
            return this;
        }

        /// <summary>
        /// add total filter to query
        /// </summary>
        /// <param name="from">from lower bound of total filter</param>
        /// <param name="to">to upper bound of total filter</param>
        /// <returns>order query</returns>
        public IOrderQuery ByTotal(decimal? from, decimal? to)
        {
            EnsureQuery();
            if (from.HasValue)
                _query = _query.Where(o => o.Total >= from.Value);
            if (to.HasValue)
                _query = _query.Where(o => o.Total <= to.Value);
            return this;
        }

        /// <summary>
        /// filter by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>order query</returns>
        public IOrderQuery ByCustomField(string customFieldName, string fieldValue)
        {
            EnsureQuery();
            var customFieldQuery = _orderService.CustomFieldsQuery().Where(o => o.Name == customFieldName && o.Value == fieldValue);
            _query = _query.Where(o => customFieldQuery.Any(c => c.OrderId == o.Id));
            return this;
        }

        /// <summary>
        /// get current logon user's last active order
        /// </summary>
        /// <param name="sessionId">current user's session id</param>
        /// <param name="user">current logon user info</param>
        /// <param name="deleteShoppingCart">whether to delete the shopping cart when order created</param>
        /// <returns>order</returns>
        public Order GetMyOrder(string sessionId, MembershipUser user, bool deleteShoppingCart = true)
        {
            var shoppingCart = string.IsNullOrEmpty(sessionId) ? null : _shoppingCartService.Query().Where(o => o.SessionId == sessionId).FirstOrDefault();
            Kooboo.Commerce.Orders.Order order = null;
            if (shoppingCart != null)
            {
                order = _orderService.Query().Where(o => o.ShoppingCartId == shoppingCart.Id).FirstOrDefault();
                if (order == null)
                    order = _orderService.CreateFromCart(shoppingCart, user, deleteShoppingCart);
            }
            else
            {
                var customer = _customerService.Query().Where(o => o.AccountId == user.UUID).FirstOrDefault();
                if (customer != null)
                {
                    order = _orderService.Query().Where(o => o.CustomerId == customer.Id && o.OrderStatus == Commerce.Orders.OrderStatus.Created).OrderByDescending(o => o.CreatedAtUtc).FirstOrDefault();
                }
            }
            if (order != null)
            {
                this.Include(o => o.Customer);
                var morder = Map(order);
                OnQueryExecuted();
                return morder;
            }
            return null;
        }

        protected override IDictionary<string, object> BuildItemHalParameters(Order data)
        {
            var paras = base.BuildItemHalParameters(data);
            var request = HttpContext.Current.Request;
            paras.Add("sessionId", request.QueryString["sessionId"]);
            paras.Add("deleteShoppingCart", request.QueryString["deleteShoppingCart"]);
            return paras;
        }

        /// <summary>
        /// create order query
        /// </summary>
        /// <returns>order query</returns>
        public IOrderQuery Query()
        {
            return this;
        }

        /// <summary>
        /// create order data access
        /// </summary>
        /// <returns>order data access</returns>
        public IOrderAccess Access()
        {
            return this;
        }

    }
}
