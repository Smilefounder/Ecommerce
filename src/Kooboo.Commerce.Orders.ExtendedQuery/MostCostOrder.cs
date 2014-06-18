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
    [Dependency(typeof(Kooboo.Commerce.ExtendedQuery.OrderQuery), Key = "MostCostOrder")]
    public class MostCostOrder : Kooboo.Commerce.ExtendedQuery.OrderQuery
    {
        public string Name
        {
            get { return "MostCostOrder"; }
        }

        public string Title
        {
            get { return "Most Cost Orders"; }
        }

        public string Description
        {
            get { return "Orders sorted by total price"; }
        }

        public ExtendedQueryParameter[] Parameters
        {
            get
            {
                return new ExtendedQueryParameter[]
                    {
                         new ExtendedQueryParameter() { Name = "Num", Description = "top number", Type = typeof(System.Int32), DefaultValue = 10 },
                         new ExtendedQueryParameter() { Name = "TotalPrice", Description = "the least total price in the order", Type = typeof(System.Decimal), DefaultValue = 100.0m }
                    };
            }
        }

        public IPagedList<OrderQueryModel> Query(IEnumerable<ExtendedQueryParameter> parameters, ICommerceDatabase db, int pageIndex, int pageSize)
        {
            if (pageIndex <= 1)
                pageIndex = 1;

            int num = 10;
            var numPara = parameters.FirstOrDefault(o => o.Name == "Num");
            if (numPara != null && numPara.Value != null && !string.IsNullOrEmpty(numPara.Value.ToString()))
                num = Convert.ToInt32(numPara.Value);
            decimal totalPrice = 100.0m;
            var totalPara = parameters.FirstOrDefault(o => o.Name == "TotalPrice");
            if (totalPara != null && totalPara.Value != null && !string.IsNullOrEmpty(totalPara.Value.ToString()))
                totalPrice = Convert.ToDecimal(totalPara.Value);


            var customerQuery = db.GetRepository<Customer>().Query();
            IQueryable<OrderQueryModel> query = db.GetRepository<Order>().Query()
                .Join(customerQuery,
                           order => order.CustomerId,
                           customer => customer.Id,
                           (order, customer) => new { Order = order, Customer = customer })
                .Select(o => new OrderQueryModel() { Customer = o.Customer, Order = o.Order })
                .Where(o => o.Order.Total > totalPrice)
                .OrderByDescending(o => o.Order.Total);
            var total = query.Count();
            var data = query.Skip(0).Take(num).ToArray();
            return new PagedList<OrderQueryModel>(data, pageIndex, pageSize, total);
        }
    }
}
