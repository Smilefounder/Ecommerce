using Kooboo.CMS.Web.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class EventCategoryMenuItemInitializer : CommerceMenuItemInitializer
    {
        protected override bool GetIsActive(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            var controller = controllerContext.RouteData.Values["controller"] as string;
            var action = controllerContext.RouteData.Values["action"] as string;

            if (!controller.Equals("Activity", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (menuItem is EventCategoryMenuItem && action.Equals("Events", StringComparison.OrdinalIgnoreCase))
            {
                var category = controllerContext.RequestContext.GetRequestValue("category");
                return !String.IsNullOrEmpty(category) && category.Equals(menuItem.RouteValues["category"].ToString(), StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }

    public class EventMenuItemInitializer : CommerceMenuItemInitializer
    {
        protected override bool GetIsActive(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            var controller = controllerContext.RouteData.Values["controller"] as string;
            var action = controllerContext.RouteData.Values["action"] as string;

            if (!controller.Equals("Activity", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (menuItem is EventMenuItem && action.Equals("List", StringComparison.OrdinalIgnoreCase))
            {
                var eventType = controllerContext.RequestContext.GetRequestValue("eventType");
                return !String.IsNullOrEmpty(eventType) && eventType.Equals(menuItem.RouteValues["eventType"].ToString(), StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}