using Kooboo.Commerce.Api.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Customers
{
    public class Address
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int CountryId { get; set; }

        [OptionalInclude]
        public Country Country { get; set; }
    }
}
