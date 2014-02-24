using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    [Serializable]
    public class CommerceDbException : Exception
    {
        public CommerceDbException() { }
        public CommerceDbException(string message) : base(message) { }
        public CommerceDbException(string message, Exception inner) : base(message, inner) { }
        protected CommerceDbException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
