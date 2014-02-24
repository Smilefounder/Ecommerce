using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.UPS
{
    [Dependency(typeof(IShipmentTrackerViews), Key = "Kooboo.Commerce.Shipping.UPS.UPSShipmentTrackerViews")]
    public class UPSShipmentTrackerViews : IShipmentTrackerViews
    {
        public string TrackerName
        {
            get
            {
                return Strings.TrackerName;
            }
        }

        public System.Web.Mvc.RedirectToRouteResult Settings()
        {
            return Routes.RedirectToAction("Settings", "Home", new { area = Strings.AreaName });
        }
    }
}