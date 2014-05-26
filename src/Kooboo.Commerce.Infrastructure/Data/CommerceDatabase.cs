﻿using Kooboo.Commerce.Events;
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

        public IEventDispatcher EventDispatcher { get; private set; }

        public EventTrackingScope EventTrackingContext { get; private set; }

        public CommerceDbContext DbContext { get; private set; }

        public CommerceDatabase(CommerceInstanceMetadata commerceInstanceMetadata, ICommerceDbProvider dbProvider, IEventDispatcher eventDispatcher)
        {
            Require.NotNull(commerceInstanceMetadata, "commerceInstanceMetadata");
            Require.NotNull(dbProvider, "dbProvider");
            Require.NotNull(eventDispatcher, "eventDispatcher");

            _commerceInstanceMetadata = commerceInstanceMetadata;
            EventDispatcher = eventDispatcher;
            EventTrackingContext = EventTrackingScope.Begin();
            DbContext = CommerceDbContext.Create(commerceInstanceMetadata, dbProvider);
        }

        ~CommerceDatabase()
        {
            Dispose(false);
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            ThrowIfDisposed();
            return new CommerceRepository<T>(this);
        }

        public ICommerceDbTransaction BeginTransaction()
        {
            ThrowIfDisposed();
            AssertNoCurrentTransaction();

            _currentTransaction = new CommerceDbTransaction(DbContext.Database.BeginTransaction(), this);

            return _currentTransaction;
        }

        public ICommerceDbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();
            AssertNoCurrentTransaction();

            _currentTransaction = new CommerceDbTransaction(DbContext.Database.BeginTransaction(isolationLevel), this);

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

        private void AssertNoCurrentTransaction()
        {
            if (_currentTransaction != null)
                throw new InvalidOperationException("Nesting transaction is not allowed. Ensure current transaction is disposed before starting new transaction.");
        }

        internal void DispatchPendingEvents()
        {
            var events = EventTrackingContext.PendingEvents.ToList();
            EventTrackingContext.Clear();

            foreach (var @event in events)
            {
                EventDispatcher.Dispatch(@event, new EventDispatchingContext(EventDispatchingPhase.OnTransactionCommitted, EventTrackingContext));
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

                EventTrackingContext.Dispose();
            }
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Commerce database was disposed.");
        }
    }
}
