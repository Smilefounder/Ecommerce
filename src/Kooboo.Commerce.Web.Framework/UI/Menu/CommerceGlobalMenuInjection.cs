using Kooboo.CMS.Sites.Extension.UI.GlobalSidebarMenu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Menu
{
    public abstract class CommerceGlobalMenuInjection : IMenuInjection
    {
        public abstract void Inject(Kooboo.Web.Mvc.Menu.Menu menu, System.Web.Mvc.ControllerContext controllerContext);

        void IMenuInjection.Inject(Kooboo.Web.Mvc.Menu.Menu menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (menu.Name == ModuleGlobalSidebarMenuItemProvider.GetGlobalSidebarMenuTemplateName("Commerce"))
            {
                var commerceRootItem = menu.Items.FirstOrDefault(it => it.Name == "Commerce");
                var tempMenu = new Kooboo.Web.Mvc.Menu.Menu("Commerce")
                {
                    Items = commerceRootItem.Items
                };

                Inject(tempMenu, controllerContext);

                commerceRootItem.Items = tempMenu.Items;
            }
        }
    }
}
