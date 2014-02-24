using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Shipping;
using Kooboo.Commerce.Web.Areas.Commerce.Models.ShipmentTrackers;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class ShipmentTrackerController : CommerceControllerBase
    {
        private IShipmentTrackerFactory _trackerFactory;
        private IShipmentTrackerViewsFactory _trackerViewsFactory;

        public ShipmentTrackerController(
            IShipmentTrackerFactory trackerFactory,
            IShipmentTrackerViewsFactory trackerViewsFactory)
        {
            _trackerFactory = trackerFactory;
            _trackerViewsFactory = trackerViewsFactory;
        }

        public ActionResult Index()
        {
            var trackers = _trackerFactory.All().Select(x =>
            {
                var model = new ShipmentTrackerRowModel
                {
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    SupportedCarriers = String.Join(", ", x.GetSupportedShippingCarriers().Select(c => c.Name))
                };

                return model;
            })
            .ToList();

            return View(trackers);
        }

        public ActionResult Settings(string name)
        {
            var view = _trackerViewsFactory.FindByTrackerName(name);
            var configUrl = Url.RouteUrl(view.Settings(), RouteValues.From(Request.QueryString));
            return Redirect(configUrl);
        }
    }
}
