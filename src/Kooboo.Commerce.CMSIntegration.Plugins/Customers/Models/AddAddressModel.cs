using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers.Models
{
    public class AddAddressModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int CountryId { get; set; }

        public bool SetDefaultShippingAddress { get; set; }

        public bool SetDefaultBillingAddress { get; set; }

        public string ReturnUrl { get; set; }
    }
}
