using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries.Grid;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs.Queries
{
    [Grid(Checkable = true, IdProperty = "Id", GridItemType = typeof(OrderGridItem))]
    public interface IOrderModel
    {
        int Id { get; set; }
    }
}
