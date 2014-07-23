using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Orders.Default
{
    public class DefaultOrdersQuery : OrderTabQuery<OrderModel>
    {
        public override string Name
        {
            get
            {
                return "Orders_Default";
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Default";
            }
        }

        public override Type ConfigType
        {
            get
            {
                return typeof(DefaultOrdersQueryConfig);
            }
        }

        public override Pagination Execute(QueryContext context)
        {
            var db = context.Instance.Database;
            var query = db.GetRepository<Order>().Query();

            var config = context.Config as DefaultOrdersQueryConfig;
            if (config != null)
            {
                if (config.Status != null)
                {
                    query = query.Where(o => o.Status == config.Status.Value);
                }
                if (!String.IsNullOrWhiteSpace(config.ProcessingStatus))
                {
                    query = query.Where(o => o.ProcessingStatus == config.ProcessingStatus);
                }
            }

            query = query.ByKeywords(context.Keywords);

            return query.OrderByDescending(o => o.Id)
                        .Paginate(context.PageIndex, context.PageSize)
                        .Transform(o => new OrderModel(o, o.Customer));
        }
    }
}