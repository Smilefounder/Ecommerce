using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Customers;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Customers.Default
{
    public class DefaultCustomersQuery : CustomerTabQuery<CustomerModel>
    {
        public override string Name
        {
            get
            {
                return "Customers_Default";
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
                return typeof(DefaultCustomersQueryConfig);
            }
        }

        public override Pagination Execute(QueryContext context)
        {
            var db = context.Instance.Database;
            var query = db.GetRepository<Customer>().Query();

            var config = context.Config as DefaultCustomersQueryConfig;
            if (config != null)
            {
                if (!String.IsNullOrWhiteSpace(config.Group))
                {
                    query = query.Where(c => c.Group == config.Group);
                }
            }

            query = query.ByKeywords(context.Keywords);

            return query.OrderByDescending(c => c.Id)
                        .Paginate(context.PageIndex, context.PageSize)
                        .Transform(c => new CustomerModel(c));
        }
    }
}