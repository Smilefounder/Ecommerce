using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Web.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Queries.Orders.Default
{
    public class DefaultOrderQuery : IOrderQuery
    {
        public string Name
        {
            get
            {
                return "AllOrders";
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
                return typeof(OrderModel);
            }
        }

        public Kooboo.Web.Mvc.Paging.IPagedList Execute(Data.CommerceInstance instance, int pageIndex, int pageSize, object config)
        {
            var db = instance.Database;

            return db.GetRepository<Order>()
                     .Query()
                     .OrderByDescending(o => o.Id)
                     .ToPagedList(pageIndex, pageSize)
                     .Transform(o => new OrderModel(o, o.Customer));
        }
    }
}