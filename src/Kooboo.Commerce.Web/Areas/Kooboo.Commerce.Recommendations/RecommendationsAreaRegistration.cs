using Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Recommendations
{
    public class RecommendationsAreaRegistration : AreaRegistration
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
                namespaces: new[] { "Kooboo.Commerce.Recommendations.Controllers" }
            );

            SqlceConfiguration.Configure();
        }
    }
}