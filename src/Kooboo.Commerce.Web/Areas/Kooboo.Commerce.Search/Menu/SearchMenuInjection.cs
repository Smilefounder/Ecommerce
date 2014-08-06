using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Menu
{
    public class SearchMenuItem : MenuItem
    {
        public SearchMenuItem()
        {
            Action = "Index";
            Area = Strings.AreaName;
        }
    }

    public class SearchMenuInjection : CommerceInstanceMenuInjection
    {
        public override void Inject(Kooboo.Web.Mvc.Menu.Menu menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            var root = new MenuItem
            {
                Text = "Search"
            };

            menu.Items.Add(root);

            root.Items.Add(new SearchMenuItem
            {
                Text = "Indexes",
                Controller = "Indexes"
            });
        }
    }
}