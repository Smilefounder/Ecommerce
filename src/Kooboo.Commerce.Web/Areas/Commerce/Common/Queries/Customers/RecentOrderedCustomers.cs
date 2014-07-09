using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Web.Mvc.Paging;

namespace Kooboo.Commerce.Customers.ExtendedQuery
{
    public class RecentOrderedCustomers : Kooboo.Commerce.ICustomerExtendedQuery
    {
        public string Name
        {
            get { return "RecentOrderedCustomer"; }
        }

        public string Title
        {
            get { return "Recent Ordered Customers"; }
        }

        public string Description
        {
            get { return "Customers who placed orders in the last days"; }
        }

        public ExtendedQueryParameter[] Parameters
        {
            get
            {
                return new ExtendedQueryParameter[]
                    {
                        new ExtendedQueryParameter() { Name = "Days", Description = "Customer Ordered Days Before", Type = typeof(System.Int32), DefaultValue = 7 }
                    };
            }
        }

        public IPagedList<CustomerQueryModel> Query(IEnumerable<ExtendedQueryParameter> parameters, ICommerceDatabase db, int pageIndex, int pageSize)
        {
            if (pageIndex <= 1)
                pageIndex = 1;

            int days = 7;
            var para = parameters.FirstOrDefault(o => o.Name == "Days");
            if (para != null && para.Value != null)
                days = Convert.ToInt32(para.Value);
            DateTime lastDate = DateTime.Today.AddDays(-1 * days);

            var orderQuery = db.GetRepository<Order>().Query()
                .Where(o => o.CreatedAtUtc > lastDate);
            IQueryable<CustomerQueryModel> query = db.GetRepository<Customer>().Query()
                .GroupJoin(orderQuery,
                           customer => customer.Id,
                           order => order.CustomerId,
                           (customer, orders) => new { Customer = customer, Orders = orders.Count() })
                .Select(o => new CustomerQueryModel() { Customer = o.Customer, OrdersCount = o.Orders })
                .OrderByDescending(o => o.Customer.Id);
            var total = query.Count();
            var data = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToArray();
            return new PagedList<CustomerQueryModel>(data, pageIndex, pageSize, total);
        }
    }
}
