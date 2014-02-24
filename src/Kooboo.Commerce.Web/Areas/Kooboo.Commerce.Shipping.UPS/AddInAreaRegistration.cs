using Kooboo.CMS.Common;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping.UPS
{
    public class AddInAreaRegistration : AreaRegistrationEx
    {
        public override string AreaName
        {
            get
            {
                return Strings.AreaName;
            }
        }

        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context)
        {
            context.MapRoute(
                name: AreaName + "_default",
                url: AreaName + "/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Kooboo.Commerce.Shipping.UPS.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
                );

            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config"));

            base.RegisterArea(context);
        }
    }
}