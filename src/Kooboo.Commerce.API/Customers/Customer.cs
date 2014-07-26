using Kooboo.Commerce.Api.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Customers
{
    /// <summary>
    /// custom api object
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// customer id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the account id of the customer
        /// </summary>
        public string AccountId { get; set; }
        /// <summary>
        /// first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// middle name
        /// </summary>
        public string MiddleName { get; set; }
        /// <summary>
        /// last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// email, required for register an account of the customer
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// gender
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Redundant field for easy query only.  The detail address information should be in the Addresses field.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// country 
        /// </summary>
        public Country Country { get; set; }

        public int? CountryId { get; set; }

        public int? ShippingAddressId { get; set; }

        public int? BillingAddressId { get; set; }

        /// <summary>
        /// the addresses of the customer
        /// a customer may has many addresses, and my be same/different to the order addresses.
        /// </summary>
        public IList<Address> Addresses { get; set; }

        public int SavingPoints { get; set; }

        /// <summary>
        /// fullname = first name + middle name + last name
        /// </summary>
        public string FullName
        {
            get { return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName); }
        }

        public IList<CustomerCustomField> CustomFields { get; set; }

        public Customer()
        {
            Addresses = new List<Address>();
            CustomFields = new List<CustomerCustomField>();
        }
    }
}
