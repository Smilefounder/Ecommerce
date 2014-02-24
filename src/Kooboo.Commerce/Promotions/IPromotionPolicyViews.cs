using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Promotions
{
    public interface IPromotionPolicyViews
    {
        string PolicyName { get; }

        RedirectToRouteResult Settings(Promotion promotion, ControllerContext controllerContext);
    }
}
