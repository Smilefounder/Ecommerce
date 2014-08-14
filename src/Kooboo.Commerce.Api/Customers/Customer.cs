using Kooboo.Commerce.Api.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Customers
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

        public string City { get; set; }

        public Country Country { get; set; }

        public int? CountryId { get; set; }

        public int? ShippingAddressId { get; set; }

        public int? BillingAddressId { get; set; }

        [OptionalInclude]
        public IList<Address> Addresses { get; set; }

        public int SavingPoints { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName); }
        }

        public IDictionary<string, string> CustomFields { get; set; }

        public Customer()
        {
            Addresses = new List<Address>();
            CustomFields = new Dictionary<string, string>();
        }
    }
}
