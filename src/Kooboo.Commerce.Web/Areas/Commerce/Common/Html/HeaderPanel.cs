using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Html
{
    public class HeaderPanel : IHtmlString
    {
        private HeaderPanelButtonCollection _buttons;

        public HtmlHelper Html { get; private set; }

        public HeaderPanel(HtmlHelper html)
        {
            Html = html;
            _buttons = new HeaderPanelButtonCollection(html);
        }

        public HeaderPanel Buttons(Action<HeaderPanelButtonCollection> buttonsBuilder)
        {
            buttonsBuilder(_buttons);
            return this;
        }

        public string ToHtmlString()
        {
            var panel = new TagBuilder("ul");
            panel.AddCssClass("header-panel");
            panel.InnerHtml = _buttons.ToHtmlString();

            return panel.ToString(TagRenderMode.Normal);
        }
    }
}
