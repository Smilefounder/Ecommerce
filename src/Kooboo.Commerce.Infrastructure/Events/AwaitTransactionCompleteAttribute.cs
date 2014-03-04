using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AwaitTransactionCompleteAttribute : Attribute
    {
        public WhenNotInTransaction WhenNotInTransaction { get; private set; }

        public AwaitTransactionCompleteAttribute()
            : this(WhenNotInTransaction.ExecuteImmediately)
        {
        }

        public AwaitTransactionCompleteAttribute(WhenNotInTransaction whenNotInTransaction)
        {
            WhenNotInTransaction = whenNotInTransaction;
        }
    }
}
