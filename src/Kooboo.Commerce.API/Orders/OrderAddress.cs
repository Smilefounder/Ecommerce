using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.Locations;

namespace Kooboo.Commerce.API.Orders
{
    /// <summary>
    /// order address
    /// </summary>
    public class OrderAddress
    {
        /// <summary>
        /// order address id
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
        /// first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// address 1
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// another address, optional
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
        /// state, optional
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

        /// <summary>
        /// create order address from customer address
        /// </summary>
        /// <param name="address">customer address</param>
        public void FromAddress(Address address)
        {
            this.CustomerId = address.CustomerId;
            this.CountryId = address.CountryId;
            this.FirstName = address.FirstName;
            this.LastName = address.LastName;
            this.Address1 = address.Address1;
            this.Address2 = address.Address2;
            this.Postcode = address.Postcode;
            this.City = address.City;
            this.State = address.State;
            this.Phone = address.Phone;
        }
    }
}
