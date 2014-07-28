using Kooboo.Commerce.Api.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Customers
{
    /// <summary>
    /// address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// address id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// customer id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// country id
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Default address for the customer.
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// the first name of contact
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// the last name of contact
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// address 1
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// another optional address
        /// </summary>
        public string Address2 { get; set; }
        /// <summary>
        /// post code
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// optional state
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// country
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// phone, optional
        /// </summary>
        public string Phone { get; set; }

    }
}
