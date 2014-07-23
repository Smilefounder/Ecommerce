using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Menu
{
    public abstract class CommerceInstanceMenuInjection : IMenuInjection
    {
        public abstract void Inject(Kooboo.Web.Mvc.Menu.Menu menu, System.Web.Mvc.ControllerContext controllerContext);

        void IMenuInjection.Inject(Kooboo.Web.Mvc.Menu.Menu menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (menu.Name == "Commerce")
            {
                Inject(menu, controllerContext);
                PrepareItems(menu.Items, new CommerceMenuItemInitializer());
            }
        }

        void PrepareItems(IEnumerable<MenuItem> items, IMenuItemInitializer initializer)
        {
            foreach (var item in items)
            {
                if (item.Initializer == null || item.Initializer.GetType() == typeof(DefaultMenuItemInitializer))
                {
                    item.Initializer = initializer;
                }
                if (item.RouteValues == null)
                {
                    // Force instantiate RouteValues property to avoid cms exception raised from SetCurrentSite
                    item.RouteValues = new System.Web.Routing.RouteValueDictionary();
                }

                PrepareItems(item.Items, initializer);
            }
        }
    }
}
