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

        /// <summary>
        /// Default address for the customer.
        /// </summary>
        public bool IsDefault { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address1 { get; set; }

        /// <summary>
        /// optional
        /// </summary>
        public string Address2 { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        /// <summary>
        /// optional
        /// </summary>
        public string State { get; set; }

        public virtual Country Country { get; set; }

        /// <summary>
        /// optional
        /// </summary>
        public string Phone { get; set; }

    }
}
