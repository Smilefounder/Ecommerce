using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class UnitOfWorkScope : IDisposable
    {
        private IEventDispatcher _dispatcher;
        private ITransactionManager _unitOfWork;
        private List<IEvent> _events;

        public ITransactionManager UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        public UnitOfWorkScope(ITransactionManager unitOfWork, IEventDispatcher dispatcher)
        {
            Require.NotNull(unitOfWork, "unitOfWork");
            Require.NotNull(dispatcher, "dispatcher");

            _unitOfWork = unitOfWork;
            _dispatcher = dispatcher;
            _events = new List<IEvent>();

            _unitOfWork.Committed += OnUnitOfWorkComitted;
            Event.RegisterEventAppliedCallback(OnEventApplied);
            UnitOfWorkScopeContext.Bind(this);
        }

        void OnEventApplied(IEvent evnt)
        {
            _events.Add(evnt);
        }

        void OnUnitOfWorkComitted(object sender, EventArgs e)
        {
            var events = _events.ToList();
            _events.Clear();

            var context = new EventDispatchingContext(EventDispatchingPhase.OnTransactionCommitted);

            foreach (var evnt in events)
            {
                _dispatcher.Dispatch(evnt, context);
            }
        }

        public static UnitOfWorkScope Current
        {
            get
            {
                return UnitOfWorkScopeContext.Current;
            }
        }

        public static UnitOfWorkScope Begin(ITransactionManager unitOfWork, IEventDispatcher dispatcher)
        {
            return new UnitOfWorkScope(unitOfWork, dispatcher);
        }

        public void Dispose()
        {
            _unitOfWork.Committed -= OnUnitOfWorkComitted;
            Event.UnregisterEventAppliedCallback(OnEventApplied);
            UnitOfWorkScopeContext.Unbind();
        }
    }
}
