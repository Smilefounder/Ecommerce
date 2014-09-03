using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Customers
{
    public class AddressModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Postcode { get; set; }

        [Required]
        public string City { get; set; }

        public string State { get; set; }

        [Required]
        public int CountryId { get; set; }

        public bool IsDefaultBillingAddress { get; set; }

        public bool IsDefaultShippingAddress { get; set; }
    }
}