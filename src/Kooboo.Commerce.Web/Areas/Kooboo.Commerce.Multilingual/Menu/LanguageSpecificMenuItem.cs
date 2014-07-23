using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual
{
    public class LanguageSpecificMenuItem : MultilingualMenuItem
    {
        public string Language { get; set; }

        public LanguageSpecificMenuItem(string language)
        {
            Language = language;
            RouteValues = new System.Web.Routing.RouteValueDictionary();
            RouteValues.Add("culture", language);
            Initializer = new LanguageSpecificMenuItemInitializer();
        }
    }
}