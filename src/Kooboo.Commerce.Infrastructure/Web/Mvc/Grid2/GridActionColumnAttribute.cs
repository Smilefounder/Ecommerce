using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Mvc.Grid2
{
    public class GridActionColumnAttribute : GridColumnAttribute
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public string ButtonText { get; set; }

        public string Icon { get; set; }

        public GridActionColumnAttribute()
        {
            GridItemColumnType = typeof(GridActionItemColumn);
        }
    }
}
