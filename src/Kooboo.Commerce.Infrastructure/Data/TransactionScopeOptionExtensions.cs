using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Kooboo.Commerce.Data
{
    static class TransactionScopeOptionExtensions
    {
        public static EventTrackingScopeOption ToEventTrackingScopeOption(this TransactionScopeOption scopeOption)
        {
            switch (scopeOption)
            {
                case TransactionScopeOption.Required:
                    return EventTrackingScopeOption.Required;
                case TransactionScopeOption.RequiresNew:
                    return EventTrackingScopeOption.RequiresNew;
                case TransactionScopeOption.Suppress:
                    return EventTrackingScopeOption.Suppress;
                default:
                    throw new NotSupportedException("Do not support TransactionScopeOption \"" + scopeOption + "\".");
            }
        }
    }
}
