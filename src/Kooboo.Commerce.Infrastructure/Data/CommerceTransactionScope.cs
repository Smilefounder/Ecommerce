using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Kooboo.Commerce.Data
{
    public class CommerceTransactionScope : IDisposable
    {
        private TransactionScope _transactionScope;
        private EventTrackingScope _eventTrackingScope;

        public CommerceTransactionScope(IEventDispatcher eventDispatcher)
            : this(eventDispatcher, TransactionScopeOption.Required)
        {
        }

        public CommerceTransactionScope(IEventDispatcher eventDispatcher, TransactionScopeOption scopeOption)
            : this(eventDispatcher, scopeOption, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted })
        {
        }

        public CommerceTransactionScope(IEventDispatcher eventDispatcher, TransactionScopeOption scopeOption, TransactionOptions transactionOptions)
        {
            _transactionScope = new TransactionScope(scopeOption, transactionOptions);
            _eventTrackingScope = new EventTrackingScope(eventDispatcher, scopeOption.ToEventTrackingScopeOption());

            Transaction.Current.TransactionCompleted += OnCurrentTransactionCompleted;
        }

        private void OnCurrentTransactionCompleted(object sender, TransactionEventArgs e)
        {
            if (e.Transaction.TransactionInformation.Status == TransactionStatus.Committed)
            {
                _eventTrackingScope.Complete();
            }
        }

        public void Complete()
        {
            _transactionScope.Complete();
        }

        public void Dispose()
        {
            try
            {
                _transactionScope.Dispose();
            }
            finally
            {
                _eventTrackingScope.Dispose();
            }
        }
    }
}
