using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Accounts;
using Kooboo.Commerce.Locations;

namespace Kooboo.Commerce.Customers
{
    public class Customer
    {
        public Customer()
        {
            Addresses = new List<Address>();
        }


        public int Id { get; set; }

        public int? AccountId { get; set; }


        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        /// <summary>
        /// Gender, 0 = mail, 1 = female, -1 = unknown.
        /// </summary>
        public Gender Gender { get; set; }

        public string Phone { get; set; }

        public int? CountryId { get; set; }

        /// <summary>
        /// Redundant field for easy query only.  The detail address information should be in the Addresses field.
        /// </summary>

        public string City { get; set; }
        public virtual Account Account { get; set; }
        public virtual Country Country { get; set; }

        /// <summary>
        /// The list of addressed used by this user.
        /// </summary>
        public virtual List<Address> Addresses { get; set; }

        public virtual CustomerLoyalty Loyalty { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName); }
        }
    }
}