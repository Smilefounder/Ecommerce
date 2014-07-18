using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries.Grid;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs.Queries
{
    [Grid(Checkable = true, IdProperty = "Id", GridItemType = typeof(ProductGridItem))]
    public interface IProductModel
    {
        int Id { get; set; }
    }
}
