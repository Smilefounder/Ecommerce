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
            return ApiContext.Database.GetRepository<Core.Order>().Query().OrderByDescending(o => o.Id);
        }

        protected override IQueryable<Core.Order> ApplyFilter(IQueryable<Core.Order> query, QueryFilter filter)
        {
            if (filter.Name == OrderFilters.ById.Name)
            {
                var id = filter.GetParameterValueOrDefault<int>("Id");
                query = query.Where(o => o.Id == id);
            }
            else if (filter.Name == OrderFilters.ByCustomerId.Name)
            {
                var customerId = filter.GetParameterValueOrDefault<int>("CustomerId");
                query = query.Where(o => o.Customer.Id == customerId);
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
                var status = filter.GetParameterValueOrDefault<string>("ProcessingStatus");
                query = query.Where(o => o.ProcessingStatus == status);
            }
            else if (filter.Name == OrderFilters.ByCouponCode.Name)
            {
                var coupon = filter.GetParameterValueOrDefault<string>("CouponCode");
                query = query.Where(o => o.Coupon == coupon);
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
                var fieldName = (string)filter.Parameters["FieldName"];
                var fieldValue = (string)filter.Parameters["FieldValue"];
                query = query.Where(o => o.CustomFields.Any(f => f.Name == fieldName && f.Value == fieldValue));
            }

            return query;
        }
    }
}
