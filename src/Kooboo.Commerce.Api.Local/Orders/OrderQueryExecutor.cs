using Kooboo.Commerce.Api.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core = Kooboo.Commerce.Orders;

namespace Kooboo.Commerce.Api.Local.Orders
{
    class OrderQueryExecutor : QueryExecutorBase<Order, Core.Order>
    {
        public OrderQueryExecutor(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Core.Order> CreateLocalQuery()
        {
            return ApiContext.Services.Orders.Query().OrderByDescending(o => o.Id);
        }

        protected override IQueryable<Core.Order> ApplyFilter(IQueryable<Core.Order> query, QueryFilter filter)
        {
            if (filter.Name == OrderFilters.ById.Name)
            {
                query = query.Where(o => o.Id == (int)filter.Parameters["Id"]);
            }
            else if (filter.Name == OrderFilters.ByCustomerId.Name)
            {
                query = query.Where(o => o.Customer.Id == (int)filter.Parameters["CustomerId"]);
            }
            else if (filter.Name == OrderFilters.ByCustomerAccountId.Name)
            {
                query = query.Where(o => o.Customer.AccountId == (string)filter.Parameters["CustomerAccountId"]);
            }
            else if (filter.Name == OrderFilters.ByUtcCreatedDate.Name)
            {
                var fromDate = (DateTime?)filter.Parameters["FromDate"];
                var toDate = (DateTime?)filter.Parameters["ToDate"];

                if (fromDate != null)
                {
                    query = query.Where(o => o.CreatedAtUtc >= fromDate.Value.Date);
                }
                if (toDate != null)
                {
                    query = query.Where(o => o.CreatedAtUtc < toDate.Value.Date.AddDays(1));
                }
            }
            else if (filter.Name == OrderFilters.ByOrderStatus.Name)
            {
                var status = (Core.OrderStatus)(int)filter.Parameters["OrderStatus"];
                query = query.Where(o => o.Status == status);
            }
            else if (filter.Name == OrderFilters.ByProcessingStatus.Name)
            {
                query = query.Where(o => o.ProcessingStatus == (string)filter.Parameters["ProcessingStatus"]);
            }
            else if (filter.Name == OrderFilters.ByCouponCode.Name)
            {
                query = query.Where(o => o.Coupon == (string)filter.Parameters["CouponCode"]);
            }
            else if (filter.Name == OrderFilters.ByTotal.Name)
            {
                var fromTotal = (decimal?)filter.Parameters["FromTotal"];
                var toTotal = (decimal?)filter.Parameters["ToTotal"];

                if (fromTotal != null)
                {
                    query = query.Where(o => o.Total >= fromTotal.Value);
                }
                if (toTotal != null)
                {
                    query = query.Where(o => o.Total <= toTotal.Value);
                }
            }
            else if (filter.Name == OrderFilters.ByCustomField.Name)
            {
                query = query.Where(o => o.CustomFields.Any(f => f.Name == (string)filter.Parameters["FieldName"] && f.Value == (string)filter.Parameters["FieldValue"]));
            }

            return query;
        }
    }
}
