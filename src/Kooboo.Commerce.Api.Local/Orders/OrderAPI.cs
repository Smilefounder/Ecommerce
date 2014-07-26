using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Membership.Models;
using Kooboo.Commerce.Api.Orders;
using Kooboo.Commerce.Api.Carts;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Carts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Api.Local.Orders
{
    /// <summary>
    /// order api
    /// </summary>
    [Dependency(typeof(IOrderApi), ComponentLifeStyle.Transient)]
    [Dependency(typeof(IOrderQuery), ComponentLifeStyle.Transient)]
    public class OrderApi : LocalCommerceQuery<Order, Kooboo.Commerce.Orders.Order>, IOrderApi
    {
        private ICommerceDatabase _db;
        private IOrderService _orderService;
        private IShoppingCartService _shoppingCartService;
        private ICustomerService _customerService;

        public OrderApi(LocalApiContext context)
        {
            _db = context.Database;
            _orderService = context.ServiceFactory.Orders;
            _shoppingCartService = context.ServiceFactory.Carts;
            _customerService = context.ServiceFactory.Customers;
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

        public int CreateFromCart(int cartId, ShoppingContext context)
        {
            var cart = _shoppingCartService.GetById(cartId);

            return _db.WithTransaction(() =>
            {
                var order = _orderService.CreateFromCart(cart, new Kooboo.Commerce.Carts.ShoppingContext
                {
                    Culture = context.Culture,
                    Currency = context.Currency,
                    CustomerId = context.CustomerId
                });

                return order.Id;
            });
        }

        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">order id</param>
        /// <returns>order query</returns>
        public IOrderQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add customer id filter to query
        /// </summary>
        /// <param name="customerId">customer id</param>
        /// <returns>order query</returns>
        public IOrderQuery ByCustomerId(int customerId)
        {
            Query = Query.Where(o => o.CustomerId == customerId);
            return this;
        }

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">account id</param>
        /// <returns>order query</returns>
        public IOrderQuery ByAccountId(string accountId)
        {
            Query = Query.Where(o => o.Customer.AccountId == accountId);
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
            if (from.HasValue)
                Query = Query.Where(o => o.CreatedAtUtc >= from.Value);
            if (to.HasValue)
                Query = Query.Where(o => o.CreatedAtUtc <= to.Value);
            return this;
        }

        /// <summary>
        /// add order status filter to query
        /// </summary>
        /// <param name="status">order status</param>
        /// <returns>order query</returns>
        public IOrderQuery ByOrderStatus(OrderStatus status)
        {
            Query = Query.Where(o => (int)o.Status == (int)status);
            return this;
        }

        /// <summary>
        /// add coupon filter to query
        /// </summary>
        /// <param name="coupon">order coupon</param>
        /// <returns>order query</returns>
        public IOrderQuery ByCoupon(string coupon)
        {
            Query = Query.Where(o => o.Coupon == coupon);
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
            if (from.HasValue)
                Query = Query.Where(o => o.Total >= from.Value);
            if (to.HasValue)
                Query = Query.Where(o => o.Total <= to.Value);
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
            var customFieldQuery = _orderService.CustomFields().Where(o => o.Name == customFieldName && o.Value == fieldValue);
            Query = Query.Where(o => customFieldQuery.Any(c => c.OrderId == o.Id));
            return this;
        }
    }
}
