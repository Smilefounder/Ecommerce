using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class MediaLibraryInitializer : CommerceMenuItemInitializer
    {
        public override Kooboo.Web.Mvc.Menu.MenuItem Initialize(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            base.Initialize(menuItem, controllerContext);

            if (menuItem.RouteValues == null)
            {
                menuItem.RouteValues = new System.Web.Routing.RouteValueDictionary();
            }

            menuItem.RouteValues.Add("owner", GetCommerceInstanceName(controllerContext));

            return menuItem;
        }
    }
}