using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.EAV.WebControls;

namespace Kooboo.Commerce.EAV.WebControls
{
    public abstract class WebControlBase : IWebControl
    {
        public abstract string Name { get; }

        public virtual IHtmlString Render(WebControlModel model, System.Web.Mvc.ViewContext viewContext)
        {
            var html = viewContext.HtmlHelper();
            return html.Partial("~/Areas/" + Strings.AreaName + "/Views/" + Name + ".cshtml", model);
        }
    }
}