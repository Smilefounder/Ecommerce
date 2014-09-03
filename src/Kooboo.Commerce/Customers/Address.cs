using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Customers
{
    public class Address : IOrphanable
    {
        public int Id { get; set; }

        [Column]
        protected int? CustomerId { get; set; }

        [Param, StringLength(50)]
        public string FirstName { get; set; }

        [Param, StringLength(50)]
        public string LastName { get; set; }

        [Param, StringLength(50)]
        public string Phone { get; set; }

        [Param, StringLength(100)]
        public string Address1 { get; set; }

        [Param, StringLength(100)]
        public string Address2 { get; set; }

        [Param, StringLength(50)]
        public string Postcode { get; set; }

        [Param, StringLength(50)]
        public string City { get; set; }

        [Param, StringLength(50)]
        public string State { get; set; }

        public int CountryId { get; set; }

        [Reference]
        public virtual Country Country { get; set; }

        bool IOrphanable.IsOrphan()
        {
            return CustomerId == null;
        }
    }
}
