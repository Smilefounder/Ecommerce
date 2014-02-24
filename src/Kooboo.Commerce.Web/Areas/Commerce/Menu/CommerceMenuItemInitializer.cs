using Kooboo.CMS.Web.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class CommerceMenuItemInitializer : AuthorizeMenuItemInitializer
    {
        protected override bool GetIsVisible(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (String.IsNullOrEmpty(controllerContext.RequestContext.GetRequestValue(HttpCommerceInstanceContext.CommerceNameKey)))
            {
                return false;
            }

            return base.GetIsVisible(menuItem, controllerContext);
        }

        public override Kooboo.Web.Mvc.Menu.MenuItem Initialize(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            var commerceName = controllerContext.RequestContext.GetRequestValue(HttpCommerceInstanceContext.CommerceNameKey);
            if (!String.IsNullOrEmpty(commerceName))
            {
                if (menuItem.RouteValues == null)
                {
                    menuItem.RouteValues = new System.Web.Routing.RouteValueDictionary();
                }

                menuItem.RouteValues.Add(HttpCommerceInstanceContext.CommerceNameKey, commerceName);
            }

            return base.Initialize(menuItem, controllerContext);
        }
    }
}