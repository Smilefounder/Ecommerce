using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders
{
    public class OrderAddress
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        [Param]
        public string FirstName { get; set; }

        [Param]
        public string LastName { get; set; }

        [Param]
        public string Phone { get; set; }

        [Param]
        public string Address1 { get; set; }

        [Param]
        public string Address2 { get; set; }

        [Param]
        public string Postcode { get; set; }

        [Param]
        public string City { get; set; }

        [Param]
        public string State { get; set; }

        [Reference]
        public Country Country { get; set; }

        public static OrderAddress CreateFrom(Address address)
        {
            return new OrderAddress
            {
                CountryId = address.CountryId,
                FirstName = address.FirstName,
                LastName = address.LastName,
                Address1 = address.Address1,
                Address2 = address.Address2,
                Postcode = address.Postcode,
                City = address.City,
                State = address.State,
                Phone = address.Phone
            };
        }
    }
}
