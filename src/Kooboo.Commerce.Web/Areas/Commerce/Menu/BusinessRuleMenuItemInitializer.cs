﻿using Kooboo.CMS.Web.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Web.Framework.UI.Menu;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class BusinessRuleMenuItemInitializer : CommerceMenuItemInitializer
    {
        protected override bool GetIsActive(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            var controller = controllerContext.RouteData.Values["controller"] as string;
            var action = controllerContext.RouteData.Values["action"] as string;

            if (!controller.Equals("Rule", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (action.Equals("List", StringComparison.OrdinalIgnoreCase))
            {
                var eventName = controllerContext.RequestContext.GetRequestValue("eventName");
                return !String.IsNullOrEmpty(eventName) && eventName.Equals(menuItem.RouteValues["eventName"].ToString(), StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}