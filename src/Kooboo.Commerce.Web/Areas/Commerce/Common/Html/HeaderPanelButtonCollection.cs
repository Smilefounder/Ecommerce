using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Html
{
    public class HeaderPanelButtonCollection : IHtmlString, IEnumerable<HeaderPanelButton>
    {
        private List<HeaderPanelButton> _buttons = new List<HeaderPanelButton>();

        public HtmlHelper Html { get; private set; }

        public int Count
        {
            get
            {
                return _buttons.Count;
            }
        }

        public HeaderPanelButtonCollection(HtmlHelper html)
        {
            Html = html;
        }

        public HeaderPanelButton Add()
        {
            var button = new HeaderPanelButton(Html);
            _buttons.Add(button);
            return button;
        }

        public HeaderPanelButton Add(string text)
        {
            return Add().WithText(text);
        }

        public HeaderPanelButton Add(string text, string icon)
        {
            return Add(text, icon, null);
        }

        public HeaderPanelButton Add(string text, string icon, string url)
        {
            return Add().WithText(text).WithIcon(icon).WithUrl(url);
        }

        public string ToHtmlString()
        {
            if (_buttons.Count == 0)
            {
                return String.Empty;
            }

            var html = new StringBuilder();

            foreach (var button in _buttons)
            {
                html.AppendLine(button.ToHtmlString());
            }

            return html.ToString();
        }

        public IEnumerator<HeaderPanelButton> GetEnumerator()
        {
            return _buttons.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
