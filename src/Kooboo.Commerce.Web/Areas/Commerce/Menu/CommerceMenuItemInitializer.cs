using Kooboo.CMS.Web.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Data;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Web.Mvc.Menu;

namespace Kooboo.Commerce.Web.Areas.Commerce.Menu
{
    public class CommerceMenuItemInitializer : AuthorizeMenuItemInitializer
    {
        protected override bool GetIsVisible(MenuItem menuItem, ControllerContext controllerContext)
        {
            var commerceName = controllerContext.RequestContext.GetRequestValue(HttpCommerceInstanceNameResolverBase.DefaultParamName);
            if (string.IsNullOrEmpty(commerceName))
            {
                var site = Site.Current;
                if (site != null)
                {
                    var customFields = site.AsActual().CustomFields;
                    if (customFields.ContainsKey("CommerceInstance"))
                    {
                        return !string.IsNullOrEmpty(customFields["CommerceInstance"]);
                    }
                }
                return false;
            }

            return base.GetIsVisible(menuItem, controllerContext);
        }

        public override MenuItem Initialize(MenuItem menuItem, ControllerContext controllerContext)
        {
            if (menuItem.RouteValues == null)
            {
                menuItem.RouteValues = new System.Web.Routing.RouteValueDictionary();
            }
            var commerceName = controllerContext.RequestContext.GetRequestValue(HttpCommerceInstanceNameResolverBase.DefaultParamName);
            if (String.IsNullOrEmpty(commerceName))
            {
                var site = Site.Current;
                if (site != null)
                {
                    var customFields = site.AsActual().CustomFields;
                    if (customFields.ContainsKey("CommerceInstance"))
                    {
                        menuItem.RouteValues["commerceName"] = customFields["CommerceInstance"];
                    }
                }
            }
            else
            {

                menuItem.RouteValues.Add(HttpCommerceInstanceNameResolverBase.DefaultParamName, commerceName);
            }

            return base.Initialize(menuItem, controllerContext);
        }
    }
}