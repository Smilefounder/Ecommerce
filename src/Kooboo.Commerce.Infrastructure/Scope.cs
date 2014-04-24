using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    /// <summary>
    /// Provides a way to access a shared instance within the same scope in the same thread.
    /// </summary>
    public class Scope<T> : IDisposable
        where T : class
    {
        // Scopes can be nested, _head keeps track of the most nested scope.
        [ThreadStatic]
        private static Scope<T> _head;

        private Scope<T> _parent;
        private T _instance;
        private bool _disposed;

        private Scope(T instance)
        {
            _parent = _head;
            _head = this;
            _instance = instance;
        }

        public static T Current
        {
            get
            {
                return _head == null ? null : _head._instance;
            }
        }

        public static Scope<T> Begin(T instance)
        {
            Require.NotNull(instance, "instance");
            return new Scope<T>(instance);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_head != this)
                    throw new InvalidOperationException("Scope is not disposed in order. Most nested scope must be disposed first.");

                _head = _parent;

                var disposable = _instance as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
