using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public interface IShipmentTrackerViewsFactory
    {
        IShipmentTrackerViews FindByTrackerName(string trackerName);
    }

    [Dependency(typeof(IShipmentTrackerViewsFactory))]
    public class ShipmentTrackerViewsFactory : IShipmentTrackerViewsFactory
    {
        private IEngine _engine;
        private List<IShipmentTrackerViews> _views;

        public ShipmentTrackerViewsFactory()
            : this(EngineContext.Current) { }

        public ShipmentTrackerViewsFactory(IEngine engine)
        {
            _engine = engine;
        }

        public IShipmentTrackerViews FindByTrackerName(string trackerName)
        {
            if (_views == null)
            {
                _views = _engine.ResolveAll<IShipmentTrackerViews>().ToList();
            }

            return _views.FirstOrDefault(x => x.TrackerName.Equals(trackerName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
