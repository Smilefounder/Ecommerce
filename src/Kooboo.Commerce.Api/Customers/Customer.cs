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

        public string Group { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public string Email { get; set; }

        public Gender Gender { get; set; }

        public int SavingPoints { get; set; }

        public int? DefaultShippingAddressId { get; set; }

        public int? DefaultBillingAddressId { get; set; }

        [OptionalInclude]
        public IList<Address> Addresses { get; set; }

        [OptionalInclude]
        public IDictionary<string, string> CustomFields { get; set; }

        public Customer()
        {
            Addresses = new List<Address>();
            CustomFields = new Dictionary<string, string>();
        }
    }
}
