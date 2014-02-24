using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2;
using Kooboo.Globalization;

namespace Kooboo.Commerce.Web.Grid2 {

    public class ActionGridColumn : GridItemColumn {

        public ActionGridColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue) {
        }

        public override IHtmlString RenderItemColumn(ViewContext viewContext) {
            var columnAttr = GridColumn.ColumnAttribute as ActionGridColumnAttribute;
            var actionName = columnAttr != null ? columnAttr.ActionName : "ActionName";
            var buttonText = columnAttr != null ? columnAttr.ButtonText : "ButtonText";
            var imageSrc = columnAttr != null ? columnAttr.ImageSrc : string.Empty;
            //
            if (!string.IsNullOrWhiteSpace(imageSrc)) { imageSrc = Kooboo.Web.Url.UrlUtility.ResolveUrl(imageSrc); }
            var href = viewContext.UrlHelper().Action(actionName, viewContext.RequestContext.AllRouteValues().Merge(GridColumn.GridModel.IdPorperty ?? "Id", DataItem.GetKey()).Merge("return", viewContext.HttpContext.Request.RawUrl));
            //
            if (string.IsNullOrWhiteSpace(imageSrc)) {
                return new HtmlString(string.Format("<a href=\"{0}\" title=\"{1}\">{1}</a>", href, buttonText.Localize()));
            } else {
                return new HtmlString(string.Format("<a href=\"{0}\" title=\"{1}\"><img class=\"icon\" src=\"{2}\" /></a>", href, buttonText.Localize(), imageSrc));
            }
        }
    }
}
