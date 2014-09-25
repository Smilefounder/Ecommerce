using Kooboo.CMS.Web.Authorizations;
using Kooboo.Commerce.Data;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class GlobalMenuItemInitializer : AuthorizeMenuItemInitializer
    {
        protected override bool GetIsVisible(MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            var visible = base.GetIsVisible(menuItem, controllerContext);
            if (visible)
            {
                visible = CommerceInstance.Current == null;
            }

            return visible;
        }
    }
}