using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Activities.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Web.Grid2;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    [Grid(IdProperty = "Id", Checkable = true, GridItemType = typeof(ActivityBindingRowModelGridItem))]
    public class ActivityBindingRowModel
    {
        public int Id { get; set; }

        [GridColumn(GridItemColumnType = typeof(EditGridActionItemColumn))]
        public string Description { get; set; }

        [GridColumn]
        public int Priority { get; set; }

        [BooleanGridColumn]
        public bool IsEnabled { get; set; }

        public bool Configurable { get; set; }
    }
}