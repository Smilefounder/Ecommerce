using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Grid2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EditorLinkedGridColumnAttribute : GridColumnAttribute
    {
        public string EditActionName { get; set; }

        public string KeyQueryStringParamName { get; set; }

        public EditorLinkedGridColumnAttribute()
        {
            EditActionName = "Edit";
            GridItemColumnType = typeof(EditGridActionItemColumn);
        }
    }
}