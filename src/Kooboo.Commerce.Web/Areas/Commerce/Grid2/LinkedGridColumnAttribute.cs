using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Grid2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LinkedGridColumnAttribute : GridColumnAttribute
    {
        public string TargetAction { get; set; }

        public string KeyQueryStringParamName { get; set; }

        public LinkedGridColumnAttribute()
        {
            TargetAction = "Edit";
            GridItemColumnType = typeof(LinkedGridItemColumn);
        }
    }
}