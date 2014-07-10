using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Customers;
using Kooboo.Web.Mvc.Paging;
using System.ComponentModel;

namespace Kooboo.Commerce.Orders.ExtendedQuery
{
    public class MostCostOrderConfig
    {
        [Description("Top number")]
        public int Num { get; set; }

        [Description("The least total price in the order")]
        public decimal TotalPrice { get; set; }

        public MostCostOrderConfig()
        {
            Num = 10;
        }
    }

    public class MostCostOrder : Kooboo.Commerce.IOrderExtendedQuery
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

        public Type ConfigModelType
        {
            get
            {
                return typeof(MostCostOrderConfig);
            }
        }

        public IPagedList<Order> Execute(CommerceInstance instance, int pageIndex, int pageSize, object config)
        {
            if (pageIndex <= 1)
                pageIndex = 1;

            var parameters = config as MostCostOrderConfig ?? new MostCostOrderConfig();
            var db = instance.Database;
            var customerQuery = db.GetRepository<Customer>().Query();
            IQueryable<Order> query = db.GetRepository<Order>().Query()
                .Join(customerQuery,
                           order => order.CustomerId,
                           customer => customer.Id,
                           (order, customer) => order)
                .Where(o => o.Total > parameters.TotalPrice)
                .OrderByDescending(o => o.Total);
            var total = query.Count();
            var data = query.Skip(0).Take(parameters.Num).ToArray();
            return new PagedList<Order>(data, pageIndex, pageSize, total);
        }
    }
}
