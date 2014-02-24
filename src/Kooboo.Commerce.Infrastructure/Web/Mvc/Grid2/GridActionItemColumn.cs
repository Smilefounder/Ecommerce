using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using System.Web;

namespace Kooboo.Commerce.Web.Mvc.Grid2
{
    public class GridActionItemColumn : GridItemColumn
    {
        public GridActionItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {
        }

        public override System.Web.IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var urlHelper = new UrlHelper(viewContext.RequestContext);
            var columnAttribute = GridColumn.ColumnAttribute as GridActionColumnAttribute;
            var routes = viewContext.RequestContext
                                    .AllRouteValues()
                                    .Merge("id", DataItem.GetKey())
                                    .Merge("return", viewContext.HttpContext.Request.RawUrl);

            var controller = columnAttribute.Controller ?? (string)viewContext.RouteData.Values["controller"];

            var url = urlHelper.Action(columnAttribute.Action, controller, routes);
            var iconHtml = String.IsNullOrEmpty(columnAttribute.Icon) ? "" : String.Format("<img class='icon {0}' src='/Images/invis.gif'/>", columnAttribute.Icon);

            return new HtmlString(string.Format("<a href='{0}'>{2} {1}</a>", url, columnAttribute.ButtonText, iconHtml));
        }
    }
}
