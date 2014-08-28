using Kooboo.Commerce.Web.Framework.UI.Menu;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Menu
{
    public class RecommendationsMenuInjection : CommerceInstanceMenuInjection
    {
        public override void Inject(Kooboo.Web.Mvc.Menu.Menu menu, System.Web.Mvc.ControllerContext controllerContext)
        {
            var recommendations = new RecommendationMenuItem
            {
                Text = "Recommendations"
            };

            menu.Items.Add(recommendations);

            recommendations.Items.Add(new RecommendationMenuItem
            {
                Text = "Settings",
                Controller = "Config",
                Action = "Jobs"
            });
        }
    }
}