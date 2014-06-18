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
    [Dependency(typeof(Kooboo.Commerce.ExtendedQuery.CustomerQuery), Key = "TopOrderedCustomers")]
    public class TopOrderedCustomers : Kooboo.Commerce.ExtendedQuery.CustomerQuery
    {
        public string Name
        {
            get { return "TopOrderedCustomers"; }
        }

        public string Title
        {
            get { return "Most Ordered Customers"; }
        }

        public string Description
        {
            get { return "return top number of customers who placed the most orders."; }
        }

        public ExtendedQueryParameter[] Parameters
        {
            get
            {
                return new ExtendedQueryParameter[]
                    {
                        new ExtendedQueryParameter() { Name = "Num", Description = "top number", Type = typeof(System.Int32), DefaultValue = 10 }
                    };
            }
        }

        public IPagedList<CustomerQueryModel> Query(IEnumerable<ExtendedQueryParameter> parameters, ICommerceDatabase db, int pageIndex, int pageSize)
        {
            if (pageIndex <= 1)
                pageIndex = 1;
            int num = 10;
            var numPara = parameters.FirstOrDefault(o => o.Name == "Num");
            if (numPara != null && numPara.Value != null && !string.IsNullOrEmpty(numPara.Value.ToString()))
                num = Convert.ToInt32(numPara.Value);

            var orderQuery = db.GetRepository<Order>().Query();
            IQueryable<CustomerQueryModel> query = db.GetRepository<Customer>().Query()
                .GroupJoin(orderQuery,
                           customer => customer.Id,
                           order => order.CustomerId,
                           (customer, orders) => new { Customer = customer, Orders = orders.Count() })
                .Select(o => new CustomerQueryModel() { Customer = o.Customer, OrdersCount = o.Orders })
                .OrderByDescending(o => o.OrdersCount);
            var total = query.Count();
            var data = query.Skip(0).Take(num).ToArray();
            return new PagedList<CustomerQueryModel>(data, pageIndex, pageSize, total);
        }
    }
}
