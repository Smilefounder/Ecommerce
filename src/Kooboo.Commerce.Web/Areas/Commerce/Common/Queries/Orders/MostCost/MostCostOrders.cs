using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Customers;
using Kooboo.Web.Mvc.Paging;
using System.ComponentModel;
using Kooboo.Commerce.Web.Framework.Queries;
using Kooboo.Commerce.Orders;

namespace Kooboo.Commerce.Web.Queries.Orders.MostCost
{
    public class MostCostOrders : IOrderQuery
    {
        public string Name
        {
            get { return "MostCostOrder"; }
        }

        public string DisplayName
        {
            get { return "Most Cost Orders"; }
        }

        public Type ConfigType
        {
            get
            {
                return typeof(MostCostOrdersConfig);
            }
        }

        public Type ResultType
        {
            get
            {
                return typeof(OrderModel);
            }
        }

        public IPagedList Execute(CommerceInstance instance, int pageIndex, int pageSize, object config)
        {
            if (pageIndex <= 1)
                pageIndex = 1;

            var parameters = config as MostCostOrdersConfig ?? new MostCostOrdersConfig();
            var db = instance.Database;
            var customerQuery = db.GetRepository<Customer>().Query();
            var query = db.GetRepository<Order>()
                          .Query()
                          .Join(customerQuery,
                               order => order.CustomerId,
                               customer => customer.Id,
                               (order, customer) => order)
                          .Where(o => o.Total > parameters.TotalPrice)
                          .Select(o => new OrderModel
                          {
                              Id = o.Id,
                              Total = o.Total,
                              OrderStatus = o.OrderStatus,
                              CreatedAtUtc = o.CreatedAtUtc,
                              CustomerFirstName = o.Customer.FirstName,
                              CustomerLastName = o.Customer.LastName
                          })
                          .OrderByDescending(o => o.Total);

            var total = query.Count();
            var data = query.Skip(0).Take(parameters.Num).ToArray();

            return new PagedList<OrderModel>(data, pageIndex, pageSize, total);
        }
    }
}
