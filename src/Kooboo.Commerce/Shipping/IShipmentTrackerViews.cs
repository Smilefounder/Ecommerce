using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping
{
    public interface IShipmentTrackerViews
    {
        string TrackerName { get; }

        RedirectToRouteResult Settings();
    }
}
