using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    /// <summary>
    /// 注册全局菜单，即实例管理、Add-in管理等。
    /// </summary>
    public class GlobalMenuInjection : CommerceMenuInjection
    {
        public override void Inject(Kooboo.Web.Mvc.Menu.MenuItem menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            // 只有在未选中某个实例时才显示全局菜单
            if (CommerceInstance.Current != null)
            {
                return;
            }

            menu.Items.Add(new CommerceMenuItem
            {
                Name = "Instances",
                Text = "Instances",
                Controller = "Instance",
                Action = "Index",
                Initializer = new GlobalMenuItemInitializer()
            });

            menu.Items.Add(new CommerceMenuItem
            {
                Name = "Add-ins",
                Text = "Add-ins",
                Controller = "AddInManagement",
                Action = "Index",
                Initializer = new GlobalMenuItemInitializer()
            });
        }
    }
}