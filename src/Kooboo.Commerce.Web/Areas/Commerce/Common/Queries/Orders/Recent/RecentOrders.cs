using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Customers;
using Kooboo.Web.Mvc.Paging;
using System.ComponentModel;
using Kooboo.Commerce.Web.Framework.Queries;
using Kooboo.Commerce.Orders;

namespace Kooboo.Commerce.Web.Queries.Orders.Recent
{
    public class RecentOrders : IOrderQuery
    {
        public string Name
        {
            get { return "RecentOrders"; }
        }

        public string DisplayName
        {
            get { return "Recent Orders"; }
        }

        public Type ConfigType
        {
            get
            {
                return typeof(RecentOrdersConfig);
            }
        }

        public Type ResultType
        {
            get
            {
                return typeof(OrderModel);
            }
        }

        public IPagedList Execute(CommerceInstance instance, int pageIndex, int pageSize, object config)
        {
            var parameters = config as RecentOrdersConfig ?? new RecentOrdersConfig();
            var lastDate = DateTime.Today.AddDays(-1 * parameters.Days);

            var db = instance.Database;
            var customerQuery = db.GetRepository<Customer>().Query();
            var query = db.GetRepository<Order>()
                          .Query()
                          .Join(customerQuery,
                               order => order.CustomerId,
                               customer => customer.Id,
                               (order, customer) => order)
                          .Where(o => o.CreatedAtUtc > lastDate)
                          .Select(o => new OrderModel
                          {
                              Id = o.Id,
                              Total = o.Total,
                              OrderStatus = o.OrderStatus,
                              CreatedAtUtc = o.CreatedAtUtc,
                              CustomerFirstName = o.Customer.FirstName,
                              CustomerLastName = o.Customer.LastName
                          })
                          .OrderByDescending(o => o.Id);

            var total = query.Count();
            var data = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToArray();

            return new PagedList<OrderModel>(data, pageIndex, pageSize, total);
        }
    }
}
