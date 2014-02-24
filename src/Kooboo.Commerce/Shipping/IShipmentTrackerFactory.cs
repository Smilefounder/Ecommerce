using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public interface IShipmentTrackerFactory
    {
        IEnumerable<IShipmentTracker> All();

        IShipmentTracker FindByName(string name);
    }

    [Dependency(typeof(IShipmentTrackerFactory))]
    public class ShipmentTrackerFactory : IShipmentTrackerFactory
    {
        private IEngine _engine;

        public ShipmentTrackerFactory()
            : this(EngineContext.Current) { }

        public ShipmentTrackerFactory(IEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<IShipmentTracker> All()
        {
            return _engine.ResolveAll<IShipmentTracker>();
        }

        public IShipmentTracker FindByName(string name)
        {
            return All().FirstOrDefault(x => x.Name == name);
        }
    }
}
