using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Rules;

namespace Kooboo.Commerce.Customers
{
    public class Customer
    {
        public Customer()
        {
            Addresses = new List<Address>();
            CustomFields = new List<CustomerCustomField>();
        }

        [Parameter(Name = "CustomerId", DisplayName = "Customer ID")]
        public int Id { get; set; }

        public string AccountId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        [Parameter(Name = "CustomerEmail", DisplayName = "Customer Email")]
        public string Email { get; set; }

        [Parameter(Name = "CustomerGender", DisplayName = "Customer Gender")]
        public Gender Gender { get; set; }

        public string Phone { get; set; }

        public int? CountryId { get; set; }

        public int? ShippingAddressId { get; set; }

        public int? BillingAddressId { get; set; }

        /// <summary>
        /// Redundant field for easy query only.  The detail address information should be in the Addresses field.
        /// </summary>
        public string City { get; set; }

        public virtual Country Country { get; set; }

        /// <summary>
        /// The list of addressed used by this user.
        /// </summary>
        public virtual List<Address> Addresses { get; set; }

        public virtual Address ShippingAddress 
        { 
            get
            {
                if(Addresses != null && ShippingAddressId.HasValue)
                {
                    return Addresses.FirstOrDefault(o => o.Id == ShippingAddressId.Value);
                }
                return null;
            }
        }

        public virtual Address BillingAddress
        {
            get
            {
                if (Addresses != null && BillingAddressId.HasValue)
                {
                    return Addresses.FirstOrDefault(o => o.Id == BillingAddressId.Value);
                }
                return null;
            }
        }

        public virtual CustomerLoyalty Loyalty { get; set; }
        public virtual ICollection<CustomerCustomField> CustomFields { get; set; }

        [Parameter(Name = "CustomerFullName", DisplayName = "Customer Fullname")]
        public string FullName
        {
            get { return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName); }
        }
    }
}