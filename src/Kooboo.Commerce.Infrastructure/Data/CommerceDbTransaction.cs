using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class CommerceDbTransaction : ICommerceDbTransaction
    {
        private bool _isDisposed;

        public bool IsCommitted { get; private set; }

        protected CommerceDatabase Database { get; private set; }

        protected DbContextTransaction UnderlyingTransaction { get; private set; }

        public CommerceDbTransaction(DbContextTransaction underlyingTransaction, CommerceDatabase database)
        {
            Require.NotNull(underlyingTransaction, "underlyingTransaction");
            Require.NotNull(database, "database");

            UnderlyingTransaction = underlyingTransaction;
            Database = database;
        }

        ~CommerceDbTransaction()
        {
            Dispose(false);
        }

        public void Commit()
        {
            ThrowIfDisposed();

            Database.SaveChanges();
            UnderlyingTransaction.Commit();

            IsCommitted = true;

            Database.DispatchPendingEvents();
        }

        public void Rollback()
        {
            ThrowIfDisposed();
            UnderlyingTransaction.Rollback();
            Database.EventTrackingContext.Clear();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Dispose(true);
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnderlyingTransaction.Dispose();
                Database.ClearCurrentTransaction();
            }
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null, "Transaction is disposed.");
        }
    }
}
