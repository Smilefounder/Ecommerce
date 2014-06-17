using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    [Serializable]
    public class UnrecognizedParameterException : Exception
    {
        public UnrecognizedParameterException() { }
        public UnrecognizedParameterException(string message) : base(message) { }
        public UnrecognizedParameterException(string message, Exception inner) : base(message, inner) { }
        protected UnrecognizedParameterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
