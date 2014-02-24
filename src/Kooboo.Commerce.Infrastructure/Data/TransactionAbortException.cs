using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    [Serializable]
    public class TransactionAbortException : Exception
    {
        public TransactionAbortException() { }
        public TransactionAbortException(string message) : base(message) { }
        public TransactionAbortException(string message, Exception inner) : base(message, inner) { }
        protected TransactionAbortException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
