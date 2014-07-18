﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Customers;
using Kooboo.Web.Mvc.Paging;
using System.ComponentModel;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Tabs.Queries.Orders.MostCost
{
    public class MostCostOrders : OrderTabQuery<OrderModel>
    {
        public override string Name
        {
            get { return "MostCostOrders"; }
        }

        public override string DisplayName
        {
            get { return "Most Cost Orders"; }
        }

        public override Type ConfigType
        {
            get
            {
                return typeof(MostCostOrdersConfig);
            }
        }

        public override Pagination Execute(QueryContext context)
        {
            var parameters = context.Config as MostCostOrdersConfig ?? new MostCostOrdersConfig();
            var db = context.Instance.Database;
            var customerQuery = db.GetRepository<Customer>().Query();
            var query = db.GetRepository<Order>()
                          .Query()
                          .ByKeywords(context.Keywords)
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

            return query.Paginate(context.PageIndex, context.PageSize);
        }
    }
}
