using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers.Models
{
    public class ChangeDefaultBillingAddressModel
    {
        public int AddressId { get; set; }

        public string ReturnUrl { get; set; }
    }
}
