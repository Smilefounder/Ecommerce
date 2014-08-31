using Kooboo.Commerce.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Customers.Models
{
    public class RegisterCustomerModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string PasswordConfirm { get; set; }

        public IDictionary<string, string> CustomFields { get; set; }

        public bool SetAuthCookie { get; set; }

        public string ReturnUrl { get; set; }
    }
}
