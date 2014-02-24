using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Exceptions;

namespace Kooboo.Commerce.Accounts.Exceptions
{
    public class CreateAccountException : CommerceException
    {
        public CreateAccountException(string message)
            : base(message)
        {
        }
    }
}