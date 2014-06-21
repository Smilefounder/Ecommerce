using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Customers;
using Kooboo.Web.Mvc.Paging;

namespace Kooboo.Commerce.Orders.ExtendedQuery
{
    public class RecentOrder : Kooboo.Commerce.ExtendedQuery.OrderQuery
    {
        public string Name
        {
            get { return "RecentOrder"; }
        }

        public string Title
        {
            get { return "Recent Orders"; }
        }

        public string Description
        {
            get { return "Orders placed in the last days"; }
        }

        public ExtendedQueryParameter[] Parameters
        {
            get
            {
                return new ExtendedQueryParameter[]
                    {
                        new ExtendedQueryParameter() { Name = "Days", Description = "Ordered Days Before", Type = typeof(System.Int32), DefaultValue = 7 }
                    };
            }
        }
        public IPagedList<OrderQueryModel> Query(IEnumerable<ExtendedQueryParameter> parameters, ICommerceDatabase db, int pageIndex, int pageSize)
        {
            if (pageIndex <= 1)
                pageIndex = 1;

            int days = 7;
            var para = parameters.FirstOrDefault(o => o.Name == "Days");
            if (para != null && para.Value != null)
                days = Convert.ToInt32(para.Value);
            DateTime lastDate = DateTime.Today.AddDays(-1 * days);

            var customerQuery = db.GetRepository<Customer>().Query();
            IQueryable<OrderQueryModel> query = db.GetRepository<Order>().Query()
                .Join(customerQuery,
                           order => order.CustomerId,
                           customer => customer.Id,
                           (order, customer) => new { Order = order, Customer = customer })
                .Select(o => new OrderQueryModel() { Customer = o.Customer, Order = o.Order })
                .Where(o => o.Order.CreatedAtUtc > lastDate)
                .OrderByDescending(o => o.Order.Id);
            var total = query.Count();
            var data = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToArray();
            return new PagedList<OrderQueryModel>(data, pageIndex, pageSize, total);
        }
    }
}
