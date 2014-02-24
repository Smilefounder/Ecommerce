using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public abstract class UnitOfWorkBase : ITransactionManager
    {
        private bool _isDisposed;

        public event EventHandler Committed;

        protected IEventDispatcher EventDispatcher { get; private set; }

        protected UnitOfWorkBase(IEventDispatcher eventDispatcher)
        {
            EventDispatcher = eventDispatcher;
            UnitOfWorkScope.Begin(this, EventDispatcher);
        }

        ~UnitOfWorkBase()
        {
            Dispose(false);
        }

        public virtual void Commit()
        {
            DoCommit();
            OnComitted();
        }

        protected abstract void DoCommit();

        protected virtual void OnComitted()
        {
            if (Committed != null)
                Committed(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            UnitOfWorkScope.Current.Dispose();
        }
    }
}
