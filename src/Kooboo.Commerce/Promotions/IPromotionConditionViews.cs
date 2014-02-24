using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Promotions
{
    public interface IPromotionConditionViews
    {
        string ConditionName { get; }

        RedirectToRouteResult Settings(Promotion promotion, PromotionCondition condition, ControllerContext controllerContext);
    }
}
