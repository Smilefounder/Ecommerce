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

namespace Kooboo.Commerce.Web.Queries.Customers.TopOrdered
{
    public class TopOrderedCustomersConfig
    {
        [Description("Top number")]
        public int Num { get; set; }

        public TopOrderedCustomersConfig()
        {
            Num = 10;
        }
    }

    public class TopOrderedCustomers : CustomerTabQuery<TopOrderedCustomer>
    {
        public override string Name
        {
            get { return "TopOrderedCustomers"; }
        }

        public override string DisplayName
        {
            get { return "Most Ordered"; }
        }

        public override Type ConfigType
        {
            get
            {
                return typeof(TopOrderedCustomersConfig);
            }
        }

        public override Pagination Execute(QueryContext context)
        {
            var parameters = context.Config as TopOrderedCustomersConfig ?? new TopOrderedCustomersConfig();
            var db = context.Instance.Database;

            var orderQuery = db.GetRepository<Order>().Query();
            IQueryable<TopOrderedCustomer> query = db.GetRepository<Customer>().Query().ByKeywords(context.Keywords)
                .GroupJoin(orderQuery,
                           customer => customer.Id,
                           order => order.CustomerId,
                           (customer, orders) => new { Customer = customer, Orders = orders.Count() })
                .Select(o => new TopOrderedCustomer { 
                    Id = o.Customer.Id, 
                    Email = o.Customer.Email,
                    FirstName = o.Customer.FirstName,
                    LastName = o.Customer.LastName,
                    OrdersCount = o.Orders 
                })
                .OrderByDescending(o => o.OrdersCount);

            return query.Paginate(context.PageIndex, context.PageSize);
        }
    }
}
