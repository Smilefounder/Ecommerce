using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public class Shipment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        [StringLength(50)]
        public string TrackingNumber { get; set; }

        [StringLength(100)]
        public string ShippingCarrierId { get; set; }

        [StringLength(100)]
        public string ShippingCarrierName { get; set; }

        [StringLength(100)]
        public string TrackerName { get; set; }

        public decimal? TotalWeight { get; set; }

        public DateTime? ShippedDateUtc { get; set; }

        public DateTime? DeliveryDateUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public virtual ICollection<ShipmentPackageItem> PackageItems { get; protected set; }

        public Shipment()
        {
            CreatedAtUtc = DateTime.UtcNow;
            PackageItems = new List<ShipmentPackageItem>();
        }
    }
}
