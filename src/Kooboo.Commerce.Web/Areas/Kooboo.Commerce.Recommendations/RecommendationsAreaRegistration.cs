using Kooboo.Commerce.Recommendations.Bootstrapping;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Recommendations
{
    public class RecommendationsAreaRegistration : AddinAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return Strings.AreaName;
            }
        }

        public override void RegisterArea(AddinAreaRegistrationContext context)
        {
            context.MapRoute(
                            name: AreaName + "_default",
                            url: AreaName + "/{controller}/{action}/{id}",
                            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                            namespaces: new[] { "Kooboo.Commerce.Recommendations.Controllers" }
                        );

            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config"));

            RecommendationEngineConfiguration.Configure();
        }
    }
}