using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models
{
    public class ChangeBillingAddressModel
    {
        public int NewBillingAddressId { get; set; }

        public string SuccessUrl { get; set; }
    }
}
