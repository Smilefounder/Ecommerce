using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Orders
{
    /// <summary>
    /// order query
    /// all query filter should return self(this) to support fluent api.
    /// </summary>
    public interface IOrderQuery : ICommerceQuery<Order>
    {
        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">order id</param>
        /// <returns>order query</returns>
        IOrderQuery ById(int id);
        /// <summary>
        /// add customer id filter to query
        /// </summary>
        /// <param name="customerId">customer id</param>
        /// <returns>order query</returns>
        IOrderQuery ByCustomerId(int customerId);

        /// <summary>
        /// add account id filter to query
        /// </summary>
        /// <param name="accountId">account id</param>
        /// <returns>order query</returns>
        IOrderQuery ByAccountId(string accountId);
        /// <summary>
        /// add create date filter to query
        /// </summary>
        /// <param name="from">from date filter</param>
        /// <param name="to">to date filter</param>
        /// <returns>order query</returns>
        IOrderQuery ByCreateDate(DateTime? from, DateTime? to);
        /// <summary>
        /// add order status filter to query
        /// </summary>
        /// <param name="status">order status</param>
        /// <returns>order query</returns>
        IOrderQuery ByOrderStatus(OrderStatus status);
        /// <summary>
        /// add coupon filter to query
        /// </summary>
        /// <param name="coupon">order coupon</param>
        /// <returns>order query</returns>
        IOrderQuery ByCoupon(string coupon);
        /// <summary>
        /// add total filter to query
        /// </summary>
        /// <param name="from">from lower bound of total filter</param>
        /// <param name="to">to upper bound of total filter</param>
        /// <returns>order query</returns>
        IOrderQuery ByTotal(decimal? from, decimal? to);
        /// <summary>
        /// filter by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>order query</returns>
        IOrderQuery ByCustomField(string customFieldName, string fieldValue);
    }
}
