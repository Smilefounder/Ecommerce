using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Shipping
{
    public class ShippingMethod
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        [Required, StringLength(100)]
        public string ShippingRateProviderName { get; set; }

        public string ShippingRateProviderData { get; set; }

        public virtual ICollection<ShippingMethodCustomField> CustomFields { get; protected set; }

        public ShippingMethod()
        {
            CustomFields = new List<ShippingMethodCustomField>();
        }

        public void Enable()
        {
            if (!IsEnabled)
            {
                IsEnabled = true;
            }
        }

        public void Disable()
        {
            if (IsEnabled)
            {
                IsEnabled = false;
            }
        }
    }
}
