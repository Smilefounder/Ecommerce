using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Html
{
    public static class HeaderPanelHtmlExtensions
    {
        public static HeaderPanel HeaderPanel(this HtmlHelper html)
        {
            return new HeaderPanel(html);
        }
    }
}