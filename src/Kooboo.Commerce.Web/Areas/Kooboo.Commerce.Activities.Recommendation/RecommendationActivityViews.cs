using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.Recommendation
{
    [Dependency(typeof(IActivityViews), Key = "Kooboo.Commerce.Activities.Recommendation.RecommendationActivityViews")]
    public class RecommendationActivityViews : IActivityViews
    {
        public string ActivityName
        {
            get
            {
                return Strings.ActivityName;
            }
        }

        public System.Web.Mvc.RedirectToRouteResult Settings(AttachedActivity attachedActivity, System.Web.Mvc.ControllerContext controllerContext)
        {
            return Routes.RedirectToAction("Settings", "Home", new { bindingId = attachedActivity.Id, area = Strings.AreaName });
        }
    }
}