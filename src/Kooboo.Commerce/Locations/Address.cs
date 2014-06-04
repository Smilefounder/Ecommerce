using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Locations
{
    public class Address
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int CountryId { get; set; }

        [Param]
        public string FirstName { get; set; }

        [Param]
        public string LastName { get; set; }

        [Param]
        public string Phone { get; set; }

        [Param]
        public string Address1 { get; set; }

        [Param]
        public string Address2 { get; set; }

        [Param]
        public string Postcode { get; set; }

        [Param]
        public string State { get; set; }

        [Param]
        public string City { get; set; }

        [Reference]
        public virtual Country Country { get; set; }
    }
}
