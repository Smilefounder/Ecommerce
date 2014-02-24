using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Promotions.Conditions.HasOneProduct
{
    [Dependency(typeof(IPromotionConditionViews), Key = "Kooboo.Commerce.Promotions.Conditions.HasOneProduct.HasOneProductConditionViews")]
    public class HasOneProductConditionViews : IPromotionConditionViews
    {
        public string ConditionName
        {
            get
            {
                return Strings.ConditionName;
            }
        }

        public System.Web.Mvc.RedirectToRouteResult Settings(Promotion promotion, PromotionCondition requirement, System.Web.Mvc.ControllerContext controllerContext)
        {
            return Routes.RedirectToAction("Editor", "Home", new {
                promotionId = promotion.Id,
                requirementId = requirement == null ? null :(int?)requirement.Id,
                area = Strings.AreaName 
            });
        }
    }
}