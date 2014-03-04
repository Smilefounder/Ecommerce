using Kooboo.CMS.Web.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class CommerceMenuItemInitializer : AuthorizeMenuItemInitializer
    {
        protected override bool GetIsVisible(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (String.IsNullOrEmpty(controllerContext.RequestContext.GetRequestValue(HttpCommerceInstanceNameResolverBase.DefaultParamName)))
            {
                return false;
            }

            return base.GetIsVisible(menuItem, controllerContext);
        }

        public override Kooboo.Web.Mvc.Menu.MenuItem Initialize(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            var commerceName = controllerContext.RequestContext.GetRequestValue(HttpCommerceInstanceNameResolverBase.DefaultParamName);
            if (!String.IsNullOrEmpty(commerceName))
            {
                if (menuItem.RouteValues == null)
                {
                    menuItem.RouteValues = new System.Web.Routing.RouteValueDictionary();
                }

                menuItem.RouteValues.Add(HttpCommerceInstanceNameResolverBase.DefaultParamName, commerceName);
            }

            return base.Initialize(menuItem, controllerContext);
        }
    }
}