﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    [Serializable]
    public class UnrecognizedComparisonOperatorException : Exception
    {
        public UnrecognizedComparisonOperatorException() { }
        public UnrecognizedComparisonOperatorException(string message) : base(message) { }
        public UnrecognizedComparisonOperatorException(string message, Exception inner) : base(message, inner) { }
        protected UnrecognizedComparisonOperatorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
