using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Web.Mvc.Paging;
using System.ComponentModel;
using Kooboo.Commerce.Web.Framework.Queries;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.Commerce.Customers;

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

    [Dependency(typeof(ICustomerQuery), Key = "TopOrderedCustomers")]
    public class TopOrderedCustomers : ICustomerQuery
    {
        public string Name
        {
            get { return "TopOrderedCustomers"; }
        }

        public string DisplayName
        {
            get { return "Most Ordered"; }
        }

        public string Description
        {
            get { return "return top number of customers who placed the most orders."; }
        }

        public Type ConfigModelType
        {
            get
            {
                return typeof(TopOrderedCustomersConfig);
            }
        }

        public Type ElementType
        {
            get
            {
                return typeof(TopOrderedCustomer);
            }
        }

        public IPagedList Execute(CommerceInstance instance, int pageIndex, int pageSize, object config)
        {
            if (pageIndex <= 1)
                pageIndex = 1;

            var parameters = config as TopOrderedCustomersConfig ?? new TopOrderedCustomersConfig();
            var db = instance.Database;

            var orderQuery = db.GetRepository<Order>().Query();
            IQueryable<TopOrderedCustomer> query = db.GetRepository<Customer>().Query()
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
            var total = query.Count();
            var data = query.Skip(0).Take(parameters.Num).ToArray();
            return new PagedList<TopOrderedCustomer>(data, pageIndex, pageSize, total);
        }
    }
}
