using Kooboo.CMS.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Framework.UI.Grid2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BooleanColumnAttribute : GridColumnAttribute
    {
        public BooleanColumnAttribute()
        {
            GridItemColumnType = typeof(BooleanGridItemColumn);
        }
    }
}