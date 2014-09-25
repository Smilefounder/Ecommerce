using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Web.Authorizations;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    [Dependency(typeof(IMenuInjection), ComponentLifeStyle.Singleton, Key = "CommerceRootMenuInjection", Order = 99)]
    public class CommerceRootMenuItemInjection : IMenuInjection
    {
        public void Inject(Kooboo.Web.Mvc.Menu.Menu menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (menu.Name == "AreasMenu" || menu.Name == "Sites")
            {
                menu.Items.Add(new MenuItem
                {
                    Name = "Commerce",
                    Text = "Commerce",
                    Initializer = new AuthorizeMenuItemInitializer(),
                    RouteValues = new System.Web.Routing.RouteValueDictionary()
                });
            }
        }
    }
}