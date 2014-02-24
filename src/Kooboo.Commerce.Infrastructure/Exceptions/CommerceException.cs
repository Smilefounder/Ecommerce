using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Exceptions
{
    public class CommerceException : Exception
    {
        public CommerceException()
        {
        }

        public CommerceException(string message)
            : base(message)
        {
        }
    }
}