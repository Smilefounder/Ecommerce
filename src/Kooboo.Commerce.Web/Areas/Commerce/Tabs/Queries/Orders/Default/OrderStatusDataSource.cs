using Kooboo.CMS.Sites.DataSource;
using Kooboo.Commerce.Orders;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Orders.Default
{
    public class OrderStatusDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var items = SelectListItems.FromEnum<OrderStatus>();
            items.Insert(0, new System.Web.Mvc.SelectListItem { Text = "", Value = "" });
            return items;
        }
    }
}