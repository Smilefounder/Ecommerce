using Kooboo.Commerce.Api.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public static class OrderQueryExtensions
    {
        public static Query<Order> ById(this Query<Order> query, int id)
        {
            return query.AddFilter(OrderFilters.ById.CreateFilter(new { Id = id }));
        }

        public static Query<Order> ByCustomerId(this Query<Order> query, int customerId)
        {
            return query.AddFilter(OrderFilters.ByCustomField.CreateFilter(new { CustomerId = customerId }));
        }

        public static Query<Order> ByCustomerAccountId(this Query<Order> query, string customerAccountId)
        {
            return query.AddFilter(OrderFilters.ByCustomerAccountId.CreateFilter(new { CustomerAccountId = customerAccountId }));
        }

        public static Query<Order> ByUtcCreatedDate(this Query<Order> query, DateTime? fromDate, DateTime? toDate)
        {
            return query.AddFilter(OrderFilters.ByUtcCreatedDate.CreateFilter(new { FromDate = fromDate, ToDate = toDate }));
        }

        public static Query<Order> ByOrderStatus(this Query<Order> query, OrderStatus status)
        {
            return query.AddFilter(OrderFilters.ByOrderStatus.CreateFilter(new { OrderStatus = status }));
        }

        public static Query<Order> ByProcessingStatus(this Query<Order> query, string status)
        {
            return query.AddFilter(OrderFilters.ByProcessingStatus.CreateFilter(new { ProcessingStatus = status }));
        }

        public static Query<Order> ByCouponCode(this Query<Order> query, string couponCode)
        {
            return query.AddFilter(OrderFilters.ByCouponCode.CreateFilter(new { CouponCode = couponCode }));
        }

        public static Query<Order> ByTotal(this Query<Order> query, decimal? fromTotal, decimal? toTotal)
        {
            return query.AddFilter(OrderFilters.ByTotal.CreateFilter(new { FromTotal = fromTotal, ToTotal = toTotal }));
        }

        public static Query<Order> ByCustomField(this Query<Order> query, string fieldName, string fieldValue)
        {
            return query.AddFilter(OrderFilters.ByCustomField.CreateFilter(new { FieldName = fieldName, FieldValue = fieldValue }));
        }
    }
}
