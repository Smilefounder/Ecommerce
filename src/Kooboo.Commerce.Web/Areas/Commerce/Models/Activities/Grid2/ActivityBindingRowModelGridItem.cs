using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities.Grid2
{
    public class ActivityBindingRowModelGridItem : GridItem
    {
        public ActivityBindingRowModelGridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {
        }

        public override IHtmlString RenderItemContainerAtts()
        {
            var model = (ActivityBindingRowModel)DataItem;
            var cssClass = model.Configurable ? "configurable" : "unconfigurable";
            cssClass += " " + (model.IsEnabled ? "enabled" : "disabled");

            return new HtmlString("class=\"" + cssClass + "\"");
        }
    }
}