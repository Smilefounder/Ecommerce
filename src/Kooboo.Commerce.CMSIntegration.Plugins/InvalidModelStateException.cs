using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins
{
    [Serializable]
    public class InvalidModelStateException : Exception
    {
        public InvalidModelStateException() { }
        public InvalidModelStateException(string message) : base(message) { }
        public InvalidModelStateException(string message, Exception inner) : base(message, inner) { }
        protected InvalidModelStateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
