using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public class CustomerIdentity
    {
        public string Email { get; set; }

        public bool IsGuest()
        {
            return String.IsNullOrEmpty(Email);
        }

        public static CustomerIdentity Guest()
        {
            return new CustomerIdentity();
        }
    }
}
