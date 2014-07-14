using Kooboo.Commerce.Data;
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

        public Pagination Execute(QueryContext context)
        {
            var db = context.Instance.Database;

            return db.GetRepository<Order>()
                     .Query()
                     .ByKeywords(context.Keywords)
                     .OrderByDescending(o => o.Id)
                     .Paginate(context.PageIndex, context.PageSize)
                     .Transform(o => new OrderModel(o, o.Customer));
        }
    }
}