using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Promotions.Policies.Default
{
    [Dependency(typeof(IPromotionPolicyViews), Key = "Kooboo.Commerce.Promotions.Policies.Default.DefaultPromotionPolicyViews")]
    public class DefaultPromotionPolicyViews : IPromotionPolicyViews
    {
        public string PolicyName
        {
            get
            {
                return Strings.PolicyName;
            }
        }

        public System.Web.Mvc.RedirectToRouteResult Settings(Promotion promotion, System.Web.Mvc.ControllerContext controllerContext)
        {
            return Routes.RedirectToAction(
                "Settings", "Home", new { promotionId = promotion.Id, area = Strings.AreaName });
        }
    }
}