using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Queries.Orders.Default
{
    public class AllOrders : OrderTabQuery<OrderModel>
    {
        public override string Name
        {
            get
            {
                return "AllOrders";
            }
        }

        public override string DisplayName
        {
            get
            {
                return "All";
            }
        }

        public override Pagination Execute(QueryContext context)
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