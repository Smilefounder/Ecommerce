using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;
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
        public string Name
        {
            get
            {
                return "AllCustomers";
            }
        }

        public string DisplayName
        {
            get
            {
                return "All";
            }
        }

        public Type ConfigType
        {
            get
            {
                return null;
            }
        }

        public Type ResultType
        {
            get
            {
                return typeof(CustomerModel);
            }
        }

        public Pagination Execute(QueryContext context)
        {
            var db = context.Instance.Database;
            var customerQuery = db.GetRepository<Customer>().Query().ByKeywords(context.Keywords);
            var orderQuery = db.GetRepository<Order>().Query();
            var customers = customerQuery
                .GroupJoin(orderQuery,
                           customer => customer.Id,
                           order => order.CustomerId,
                           (customer, orders) => new { Customer = customer, Orders = orders.Count() })
                .OrderByDescending(groupedItem => groupedItem.Customer.Id)
                .Paginate(context.PageIndex, context.PageSize)
                .Transform(o => new CustomerModel(o.Customer));

            return customers;
        }
    }
}