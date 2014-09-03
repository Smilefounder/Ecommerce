using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Customers;
using Kooboo.Web.Mvc.Paging;
using System.ComponentModel;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Orders.Recent
{
    public class RecentOrders : OrderTabQuery<OrderModel>
    {
        public override string Name
        {
            get { return "RecentOrders"; }
        }

        public override string DisplayName
        {
            get { return "Recent Orders"; }
        }

        public override Type ConfigType
        {
            get
            {
                return typeof(RecentOrdersConfig);
            }
        }

        public override Pagination Execute(QueryContext context)
        {
            var parameters = context.Config as RecentOrdersConfig ?? new RecentOrdersConfig();
            var lastDate = DateTime.Today.AddDays(-1 * parameters.Days).ToUniversalTime();

            var db = context.Instance.Database;
            var customerQuery = db.Repository<Customer>().Query();
            var query = db.Repository<Order>()
                          .Query()
                          .ByKeywords(context.Keywords)
                          .Join(customerQuery,
                               order => order.CustomerId,
                               customer => customer.Id,
                               (order, customer) => order)
                          .Where(o => o.CreatedAtUtc > lastDate)
                          .Select(o => new OrderModel
                          {
                              Id = o.Id,
                              Total = o.Total,
                              Status = o.Status,
                              CreatedAtUtc = o.CreatedAtUtc,
                              CustomerFirstName = o.Customer.FirstName,
                              CustomerLastName = o.Customer.LastName
                          })
                          .OrderByDescending(o => o.Id);

            return query.Paginate(context.PageIndex, context.PageSize);
        }
    }
}
