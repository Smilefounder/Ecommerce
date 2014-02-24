using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public interface IShipmentTracker
    {
        string Name { get; }

        string DisplayName { get; }

        IEnumerable<ShippingCarrier> GetSupportedShippingCarriers();

        IEnumerable<ShipmentStatusEvent> GetShipmentStatusEvents(string trackingNumber);
    }
}
