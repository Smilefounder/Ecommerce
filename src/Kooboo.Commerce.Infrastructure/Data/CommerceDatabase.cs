using Kooboo.Commerce.Events;
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
        private InstanceMetadata _instanceMetadata;

        public InstanceMetadata InstanceMetadata
        {
            get { return _instanceMetadata; }
        }

        public ICommerceDbTransaction Transaction
        {
            get
            {
                return _currentTransaction;
            }
        }

        public CommerceDbContext DbContext { get; private set; }

        public CommerceDatabase(InstanceMetadata commerceInstanceMetadata, ICommerceDbProvider dbProvider)
        {
            Require.NotNull(commerceInstanceMetadata, "commerceInstanceMetadata");
            Require.NotNull(dbProvider, "dbProvider");

            _instanceMetadata = commerceInstanceMetadata;
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

        public void SaveChanges()
        {
            var transaction = Transaction;
            if (transaction == null)
            {
                using (transaction = BeginTransaction())
                {
                    DbContext.SaveChanges();
                    transaction.Commit();
                }
            }
            else
            {
                DbContext.SaveChanges();
            }
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
