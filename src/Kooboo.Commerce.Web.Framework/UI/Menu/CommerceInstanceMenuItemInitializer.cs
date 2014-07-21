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

namespace Kooboo.Commerce.Web.Framework.UI.Menu
{
    public class CommerceMenuItemInitializer : AuthorizeMenuItemInitializer
    {
        protected override bool GetIsVisible(MenuItem menuItem, ControllerContext controllerContext)
        {
            var instanceName = GetCommerceInstanceName(controllerContext);
            return !String.IsNullOrWhiteSpace(instanceName);
        }

        public override MenuItem Initialize(MenuItem menuItem, ControllerContext controllerContext)
        {
            if (menuItem.RouteValues == null)
            {
                menuItem.RouteValues = new System.Web.Routing.RouteValueDictionary();
            }
            var instanceName = GetCommerceInstanceName(controllerContext);
            if (!String.IsNullOrWhiteSpace(instanceName))
            {
                menuItem.RouteValues["instance"] = instanceName;
            }

            return base.Initialize(menuItem, controllerContext);
        }

        protected string GetCommerceInstanceName(ControllerContext controllerContext)
        {
            var instance = controllerContext.RequestContext.GetRequestValue("instance");
            if (!String.IsNullOrWhiteSpace(instance))
            {
                return instance;
            }

            var site = Site.Current;
            if (site == null)
            {
                return null;
            }

            var customFields = site.AsActual().CustomFields;
            if (customFields.TryGetValue("CommerceInstance", out instance))
            {
                return instance;
            }

            return null;
        }
    }
}