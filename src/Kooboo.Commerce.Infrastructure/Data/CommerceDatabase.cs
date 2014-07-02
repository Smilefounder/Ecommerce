using Kooboo.Commerce.Data.Providers;
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

        public CommerceInstanceSettings InstanceSettings { get; private set; }

        public ICommerceDbTransaction Transaction
        {
            get
            {
                return _currentTransaction;
            }
        }

        public CommerceDbContext DbContext { get; private set; }

        public CommerceDatabase(CommerceInstanceSettings instanceSettings)
            : this(instanceSettings, null)
        {
        }

        public CommerceDatabase(CommerceInstanceSettings instanceSettings, ICommerceDbProvider dbProvider)
        {
            Require.NotNull(instanceSettings, "instanceSettings");

            if (dbProvider == null)
            {
                dbProvider = CommerceDbProviders.Providers.Find(instanceSettings.DbProviderInvariantName, instanceSettings.DbProviderManifestToken);
                if (dbProvider == null)
                    throw new InvalidOperationException("Cannot find db provider from the provided manifest. Invariant name: " + instanceSettings.DbProviderInvariantName + ", manifest token: " + instanceSettings.DbProviderManifestToken + ".");
            }

            InstanceSettings = instanceSettings;
            DbContext = CommerceDbContext.Create(instanceSettings, dbProvider);
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
