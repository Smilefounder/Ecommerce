using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Payments.PayPal
{
    public class AddInAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return Strings.AreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: AreaName + "_default",
                url: AreaName + "/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Kooboo.Commerce.Payments.PayPal.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );

            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config"));
        }
    }
}