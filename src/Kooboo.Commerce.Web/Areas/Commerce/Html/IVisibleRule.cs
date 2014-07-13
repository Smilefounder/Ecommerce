using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Html
{
    public interface IVisibleRule
    {
        void Apply(TagBuilder button);
    }

    public class ByCheckedVisibleRule : IVisibleRule
    {
        public GridChecked CheckType { get; set; }

        public void Apply(TagBuilder button)
        {
            button.MergeAttribute("data-show-on-check", CheckType.ToString());
        }
    }

    public class ByCssSelectorVisibleRule : IVisibleRule
    {
        public string Selector { get; set; }

        public void Apply(TagBuilder button)
        {
            button.MergeAttribute("data-show-on-check", "Any");
            button.MergeAttribute("data-show-on-selector", Selector);
        }
    }
}
