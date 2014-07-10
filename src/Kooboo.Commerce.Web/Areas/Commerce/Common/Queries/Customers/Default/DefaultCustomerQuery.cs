using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Customers;
using Kooboo.Commerce.Web.Framework.Queries;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Queries.Customers.Default
{
    public class DefaultCustomerQuery : ICustomerQuery
    {
        public IPagedList Execute(Data.CommerceInstance instance, int pageIndex, int pageSize, object config)
        {
            var db = instance.Database;
            var customerQuery = db.GetRepository<Customer>().Query();
            var orderQuery = db.GetRepository<Order>().Query();
            var customers = customerQuery
                .GroupJoin(orderQuery,
                           customer => customer.Id,
                           order => order.CustomerId,
                           (customer, orders) => new { Customer = customer, Orders = orders.Count() })
                .OrderByDescending(groupedItem => groupedItem.Customer.Id)
                .ToPagedList(pageIndex, pageSize)
                .Transform(o => new CustomerModel(o.Customer));

            return customers;
        }

        public string Name
        {
            get
            {
                return "All";
            }
        }

        public string DisplayName
        {
            get
            {
                return "All";
            }
        }

        public Type ConfigModelType
        {
            get
            {
                return null;
            }
        }

        public Type ElementType
        {
            get
            {
                return typeof(CustomerModel);
            }
        }
    }
}