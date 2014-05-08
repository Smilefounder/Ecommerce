using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kooboo.Globalization;

namespace Kooboo.Commerce.Web.Html
{
    public class HeaderPanelButton : IHtmlString
    {
        private List<IVisibleRule> _visibleRules = new List<IVisibleRule>();
        private CommandType _commandType;
        private string _primaryIcon;
        private string _secondaryIcon;
        private string _text;
        private string _url;
        private string _confirm;
        private bool _isAjaxFormTrigger;
        private string _ajaxFormId = null;
        private string _customHtml;

        private HeaderPanelButtonCollection _dropdownItems;

        public HtmlHelper Html { get; private set; }

        public HeaderPanelButton(HtmlHelper html)
        {
            Html = html;
            _dropdownItems = new HeaderPanelButtonCollection(html);
        }

        public HeaderPanelButton WithCommandType(CommandType commandType)
        {
            _commandType = commandType;
            return this;
        }

        public HeaderPanelButton WithIcon(string icon)
        {
            _primaryIcon = icon;
            return this;
        }

        public HeaderPanelButton WithSecondaryIcon(string icon)
        {
            _secondaryIcon = icon;
            return this;
        }

        public HeaderPanelButton WithText(string text)
        {
            _text = text;
            return this;
        }

        public HeaderPanelButton WithUrl(string url)
        {
            _url = url;
            return this;
        }

        public HeaderPanelButton WithConfirmMessage(string confirmMessage)
        {
            _confirm = confirmMessage;
            return this;
        }

        public HeaderPanelButton VisibleWhen(GridChecked type)
        {
            _visibleRules.Add(new ByCheckedVisibleRule { CheckType = type });
            return this;
        }

        public HeaderPanelButton VisibleWhenSelected(string selector)
        {
            _visibleRules.Add(new ByCssSelectorVisibleRule { Selector = selector });
            return this;
        }

        public HeaderPanelButton AsAjaxFormTrigger(string ajaxFormId = null)
        {
            _isAjaxFormTrigger = true;
            _ajaxFormId = ajaxFormId;
            return this;
        }

        public HeaderPanelButton WithCustomHtml(string html)
        {
            _customHtml = html;
            return this;
        }

        public HeaderPanelButton Dropdown(Action<HeaderPanelButtonCollection> dropdownBuilder)
        {
            dropdownBuilder(_dropdownItems);
            return this;
        }

        public string ToHtmlString()
        {
            if (_customHtml != null)
            {
                return _customHtml;
            }

            var item = new TagBuilder("li");

            if (_dropdownItems.Count > 0)
            {
                item.AddCssClass("j_DropDown");

                if (String.IsNullOrEmpty(_secondaryIcon))
                {
                    WithSecondaryIcon("arrow chevron-down-white");
                }
            }

            var button = CreateButton();
            var innerHtml = button.ToString();

            if (_dropdownItems.Count > 0)
            {
                innerHtml += "<ul class=\"j_DropDownContent hide\">";
                innerHtml += _dropdownItems.ToHtmlString();
                innerHtml += "</ul>";
            }

            item.InnerHtml = innerHtml;

            return item.ToString();
        }

        private TagBuilder CreateButton()
        {
            var button = new TagBuilder("a");

            if (_isAjaxFormTrigger)
            {
                button.Attributes.Add("data-ajaxform", "");
            }

            if (!String.IsNullOrEmpty(_url))
            {
                button.Attributes.Add("href", _url);
            }

            foreach (var visibleRule in _visibleRules)
            {
                visibleRule.Apply(button);
            }

            if (_commandType != CommandType.None)
            {
                button.MergeAttribute("data-command-type", _commandType.ToString());
            }
            if (!String.IsNullOrEmpty(_confirm))
            {
                button.MergeAttribute("data-confirm", _confirm.Localize());
            }

            var buttonInnerHtml = RenderIcon(_primaryIcon) ?? String.Empty;

            if (!String.IsNullOrEmpty(_text))
            {
                buttonInnerHtml += _text.Localize();
            }

            buttonInnerHtml += RenderIcon(_secondaryIcon) ?? String.Empty;

            button.InnerHtml = buttonInnerHtml;

            return button;
        }

        private string RenderIcon(string icon)
        {
            if (!String.IsNullOrEmpty(icon))
            {
                var iconTag = new TagBuilder("img");
                iconTag.AddCssClass("icon " + icon);
                iconTag.Attributes.Add("src", "/Images/invis.gif");

                return iconTag.ToString(TagRenderMode.SelfClosing);
            }

            return null;
        }
    }

}
