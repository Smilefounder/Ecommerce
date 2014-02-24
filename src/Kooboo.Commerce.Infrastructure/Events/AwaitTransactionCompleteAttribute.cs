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
        public WhenNoInTransaction WhenNotInTransaction { get; private set; }

        public AwaitTransactionCompleteAttribute()
            : this(WhenNoInTransaction.ExecuteImmediately)
        {
        }

        public AwaitTransactionCompleteAttribute(WhenNoInTransaction whenNotInTransaction)
        {
            WhenNotInTransaction = whenNotInTransaction;
        }
    }
}
