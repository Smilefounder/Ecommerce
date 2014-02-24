using Kooboo.Commerce.Promotions;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions.Grid2
{
    public class PromotionRowModelGridItem : GridItem
    {
        public PromotionRowModelGridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {
        }

        public override IHtmlString RenderItemContainerAtts()
        {
            var model = (PromotionRowModel)DataItem;
            var cssClass = model.IsEnabled ? "enabled" : "disabled";
            return new HtmlString("class=\"" + cssClass + "\"");
        }
    }
}