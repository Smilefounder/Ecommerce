using Kooboo.Commerce.API;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers.Models
{
    public class RegisterCustomerModel : SubmissionModel
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int? CountryId { get; set; }

        public string City { get; set; }

        public IDictionary<string, string> CustomFields { get; set; }

        public bool SetAuthCookie { get; set; }
    }
}
