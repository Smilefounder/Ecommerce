using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class CommerceDatabase : ICommerceDatabase
    {
        private bool _isDisposed;
        private CommerceDbTransaction _currentTransaction;
        private CommerceInstanceMetadata _commerceInstanceMetadata;

        public CommerceInstanceMetadata CommerceInstanceMetadata
        {
            get { return _commerceInstanceMetadata; }
        }

        public ICommerceDbTransaction Transaction
        {
            get
            {
                return _currentTransaction;
            }
        }

        public CommerceDbContext DbContext { get; private set; }

        public CommerceDatabase(CommerceInstanceMetadata commerceInstanceMetadata, ICommerceDbProvider dbProvider)
        {
            Require.NotNull(commerceInstanceMetadata, "commerceInstanceMetadata");
            Require.NotNull(dbProvider, "dbProvider");

            _commerceInstanceMetadata = commerceInstanceMetadata;
            DbContext = CommerceDbContext.Create(commerceInstanceMetadata, dbProvider);
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            ThrowIfDisposed();
            return new CommerceRepository<T>(this);
        }

        public ICommerceDbTransaction BeginTransaction()
        {
            ThrowIfDisposed();

            if (_currentTransaction == null)
            {
                _currentTransaction = new CommerceDbTransaction(DbContext.Database.BeginTransaction(), this);
            }

            return _currentTransaction;
        }

        public ICommerceDbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            if (_currentTransaction == null)
            {
                _currentTransaction = new CommerceDbTransaction(DbContext.Database.BeginTransaction(isolationLevel), this);
            }

            return _currentTransaction;
        }

        public int SaveChanges()
        {
            var result = 0;
            var transaction = Transaction;
            if (transaction == null)
            {
                using (transaction = BeginTransaction())
                {
                    result = DbContext.SaveChanges();
                    transaction.Commit();
                }
            }
            else
            {
                result = DbContext.SaveChanges();
            }

            return result;
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

        internal void ClearCurrentTransaction()
        {
            _currentTransaction = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext.Dispose();

                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                }
            }
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Commerce database was disposed.");
        }
    }
}
