using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.OrderInvoiceMailing
{
    [Dependency(typeof(IActivityViews), Key = "Kooboo.Commerce.Activities.OrderInvoiceMailing.ActivityViews")]
    public class ActivityViews : IActivityViews
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
            return Routes.RedirectToAction("Settings", "Home", new { ruleId = attachedActivity.Rule.Id, attachedActivityId = attachedActivity.Id, area = Strings.AreaName });
        }
    }
}