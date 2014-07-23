using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Web.Mvc.Paging;
using System.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Customers.RecentOrdered
{
    public class RecentOrderedCustomers : CustomerTabQuery<RecentOrderedCustomer>
    {
        public override string Name
        {
            get { return "RecentOrderedCustomers"; }
        }

        public override string DisplayName
        {
            get
            {
                return "Recent Ordered";
            }
        }

        public override Type ConfigType
        {
            get
            {
                return typeof(RecentOrderedCustomersConfig);
            }
        }

        public override Pagination Execute(QueryContext context)
        {
            var config = context.Config as RecentOrderedCustomersConfig ?? new RecentOrderedCustomersConfig();
            var db = context.Instance.Database;
            var lastDate = DateTime.Today.AddDays(-1 * config.Days);

            var orderQuery = db.GetRepository<Order>().Query().Where(o => o.CreatedAtUtc > lastDate);

            var query = db.GetRepository<Customer>()
                          .Query()
                          .ByKeywords(context.Keywords)
                          .GroupJoin(orderQuery,
                                       customer => customer.Id,
                                       order => order.CustomerId,
                                       (customer, orders) => new { Customer = customer, Orders = orders.Count() })
                          .Select(o => new RecentOrderedCustomer
                          {
                              Id = o.Customer.Id,
                              FirstName = o.Customer.FirstName,
                              LastName = o.Customer.LastName,
                              Email = o.Customer.Email,
                              OrdersCount = o.Orders
                          })
                          .OrderByDescending(o => o.Id);

            return query.Paginate(context.PageIndex, context.PageSize);
        }
    }
}
