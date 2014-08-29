using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual
{
    public class MultilingualMenuItem : CommerceMenuItem
    {
        public MultilingualMenuItem()
        {
            Area = Strings.AreaName;
        }
    }
}