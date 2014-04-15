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
    public class LinkedGridItemColumn : GridItemColumn
    {
        public LinkedGridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
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

            var columnAttr = GridColumn.ColumnAttribute as LinkedGridColumnAttribute;
            var editActionName = columnAttr == null ? "Edit" : columnAttr.TargetAction;

            var idProperty = GridColumn.GridModel.IdPorperty ?? "Id";
            var extraRouteValues = viewContext.RequestContext.AllRouteValues().Merge(idProperty, GetDataItemId(DataItem, idProperty));
            extraRouteValues = extraRouteValues.Merge("return", viewContext.HttpContext.Request.RawUrl);

            var url = viewContext.UrlHelper().Action(editActionName, extraRouteValues);

            return new HtmlString(string.Format("<a href='{0}'><img class='icon {2}' src='{3}'/> {1}</a>", url, linkText,
                @class, Kooboo.Web.Url.UrlUtility.ResolveUrl("~/Images/invis.gif")));
        }

        private object GetDataItemId(object dataItem, string idProperty)
        {
            var property = dataItem.GetType().GetProperty(idProperty, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (property == null)
                throw new InvalidOperationException("Cannot find id property '" + idProperty + "' from type " + dataItem.GetType() + ".");

            return property.GetValue(dataItem, null);
        }

        protected virtual string Class
        {
            get { return ""; }
        }
    }
}