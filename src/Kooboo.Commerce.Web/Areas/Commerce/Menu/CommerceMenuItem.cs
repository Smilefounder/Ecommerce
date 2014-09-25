using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class CommerceMenuItem : MenuItem
    {
        public CommerceMenuItem()
        {
            Area = "Commerce";
            RouteValues = new System.Web.Routing.RouteValueDictionary();
        }
    }
}