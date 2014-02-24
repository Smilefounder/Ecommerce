using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ProductTypes.Grid2
{
    public class ProductTypeRowModelGridItem : GridItem
    {
        public ProductTypeRowModelGridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {
        }

        public override IHtmlString RenderItemContainerAtts()
        {
            var model = (ProductTypeRowModel)DataItem;
            return MvcHtmlString.Create(String.Format("class='{0}'", model.IsEnabled ? "enabled" : "disabled"));
        }
    }
}