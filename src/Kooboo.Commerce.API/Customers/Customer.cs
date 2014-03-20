using Kooboo.Commerce.API.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Customers
{
    public class Customer
    {
        public int Id { get; set; }

        public string AccountId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public string Phone { get; set; }

        /// <summary>
        /// Redundant field for easy query only.  The detail address information should be in the Addresses field.
        /// </summary>
        public string City { get; set; }

        public Country Country { get; set; }
        public Address[] Addresses { get; set; }

        public CustomerLoyalty Loyalty { get; set; }
        public string FullName
        {
            get { return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName); }
        }
    }
}
