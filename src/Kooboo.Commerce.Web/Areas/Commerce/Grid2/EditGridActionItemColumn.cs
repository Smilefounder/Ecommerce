#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Commerce.Web.Areas.Commerce;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Kooboo.Commerce.Web.Grid2
{
    public class EditGridActionItemColumn : GridItemColumn
    {
        public EditGridActionItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            var linkText = "Edit".Localize();
            var @class = Class;
            if (!string.IsNullOrEmpty(this.GridColumn.PropertyName))
            {
                linkText = this.PropertyValue == null ? "" : PropertyValue.ToString();
            }

            var columnAttr = GridColumn.ColumnAttribute as EditorLinkedGridColumnAttribute;
            var editActionName = columnAttr == null ? "Edit" : columnAttr.EditActionName;

            var extraRouteValues = viewContext.RequestContext.AllRouteValues().Merge(GridColumn.GridModel.IdPorperty ?? "Id", EntityUtil.GetKey(DataItem));
            extraRouteValues = extraRouteValues.Merge("return", viewContext.HttpContext.Request.RawUrl);

            var url = viewContext.UrlHelper().Action(editActionName, extraRouteValues);

            return new HtmlString(string.Format("<a href='{0}'><img class='icon {2}' src='{3}'/> {1}</a>", url, linkText,
                @class, Kooboo.Web.Url.UrlUtility.ResolveUrl("~/Images/invis.gif")));
        }

        protected virtual string Class
        {
            get { return ""; }
        }
    }
}