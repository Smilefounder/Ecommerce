using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Multilingual
{
    public class LanguageSpecificMenuItemInitializer : CommerceMenuItemInitializer
    {
        protected override bool GetIsActive(MenuItem menuItem, ControllerContext controllerContext)
        {
            var isActive = base.GetIsActive(menuItem, controllerContext);
            if (isActive)
            {
                var culture = menuItem.RouteValues["culture"] as string;
                if (!String.IsNullOrEmpty(culture) && culture.Equals(controllerContext.HttpContext.Request.QueryString["culture"], StringComparison.OrdinalIgnoreCase))
                {
                    isActive = true;
                }
                else
                {
                    isActive = false;
                }
            }

            return isActive;
        }
    }
}