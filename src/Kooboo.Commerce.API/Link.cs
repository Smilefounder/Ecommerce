using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.Commerce.API
{
    /// <summary>
    /// Represents a HAL link.
    /// </summary>
    public class Link
    {
        public string Rel { get; set; }

        public string Href { get; set; }

        public bool IsTemplated
        {
            get { return !string.IsNullOrEmpty(Href) && TemplateParamPattern.IsMatch(Href); }
        }

        static readonly Regex TemplateParamPattern = new Regex(@"{.+}", RegexOptions.Compiled);
    }
}
