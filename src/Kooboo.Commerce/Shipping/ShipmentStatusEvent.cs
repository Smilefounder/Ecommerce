using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public class ShipmentStatusEvent
    {
        public DateTime DateUtc { get; set; }

        public string Description { get; set; }

        public string CountryCode { get; set; }

        public string Location { get; set; }
    }
}
