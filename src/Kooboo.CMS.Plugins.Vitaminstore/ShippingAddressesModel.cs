using Kooboo.Commerce.Api.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Plugins.Vitaminstore
{
    public class ShippingAddressesModel
    {
        public Address Default { get; set; }

        public IList<Address> Alternatives { get; set; }

        public ShippingAddressesModel()
        {
            Alternatives = new List<Address>();
        }
    }
}
