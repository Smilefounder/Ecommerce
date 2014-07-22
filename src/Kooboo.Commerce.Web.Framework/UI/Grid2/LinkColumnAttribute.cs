using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Framework.UI.Grid2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LinkColumnAttribute : GridColumnAttribute
    {
        public string TargetAction { get; set; }

        public string IdParamName { get; set; }

        public LinkColumnAttribute(string targetAction)
        {
            TargetAction = targetAction;
            GridItemColumnType = typeof(LinkGridItemColumn);
        }
    }
}