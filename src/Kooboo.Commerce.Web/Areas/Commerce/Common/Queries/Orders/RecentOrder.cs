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
    public class RecentOrderConfig
    {
        [Description("Ordered days before")]
        public int Days { get; set; }

        public RecentOrderConfig()
        {
            Days = 7;
        }
    }

    public class RecentOrder : Kooboo.Commerce.IOrderExtendedQuery
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

        public Type ConfigModelType
        {
            get
            {
                return typeof(RecentOrderConfig);
            }
        }

        public IPagedList<Order> Execute(CommerceInstance instance, int pageIndex, int pageSize, object config)
        {
            if (pageIndex <= 1)
                pageIndex = 1;

            var parameters = config as RecentOrderConfig ?? new RecentOrderConfig();

            DateTime lastDate = DateTime.Today.AddDays(-1 * parameters.Days);

            var db = instance.Database;
            var customerQuery = db.GetRepository<Customer>().Query();
            IQueryable<Order> query = db.GetRepository<Order>().Query()
                .Join(customerQuery,
                           order => order.CustomerId,
                           customer => customer.Id,
                           (order, customer) => order)
                .Where(o => o.CreatedAtUtc > lastDate)
                .OrderByDescending(o => o.Id);
            var total = query.Count();
            var data = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToArray();
            return new PagedList<Order>(data, pageIndex, pageSize, total);
        }
    }
}
