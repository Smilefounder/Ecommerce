using Kooboo.Commerce.Events.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.Events.Registry
{
    public class DefaultEventRegistry : IEventRegistry
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly HashSet<Type> _uncategorizedEvents = new HashSet<Type>();
        private readonly Dictionary<string, List<Type>> _eventTypesByCategory = new Dictionary<string, List<Type>>();
        private Dictionary<Type, List<MethodInfo>> _handlerMethodsByEventType = new Dictionary<Type, List<MethodInfo>>();

        #region Find Events

        public IEnumerable<string> AllEventCategories()
        {
            IEnumerable<string> categories = null;

            _lock.EnterReadLock();

            try
            {
                categories = _eventTypesByCategory.Keys;
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return categories;
        }

        public IEnumerable<Type> AllEvents()
        {
            IEnumerable<Type> types = null;

            _lock.EnterReadLock();
            try
            {
                types = _handlerMethodsByEventType.Keys;
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return types;
        }

        public IEnumerable<Type> FindUncategorizedEvents()
        {
            IEnumerable<Type> types = null;

            _lock.EnterReadLock();

            try
            {
                types = _uncategorizedEvents.ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return types;
        }

        public IEnumerable<Type> FindEventsByCategory(string category)
        {
            _lock.EnterReadLock();

            try
            {
                List<Type> eventTypes;

                if (_eventTypesByCategory.TryGetValue(category, out eventTypes))
                {
                    return eventTypes.ToList();
                }

                return Enumerable.Empty<Type>();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        #endregion

        #region Find Handlers

        public IEnumerable<MethodInfo> FindHandlerMethods(Type eventType)
        {
            Require.NotNull(eventType, "eventType");

            var handlerMethods = new List<MethodInfo>();

            var holder = _handlerMethodsByEventType;

            _lock.EnterReadLock();

            try
            {
                handlerMethods = FindDirectHandlers(eventType, holder).ToList();

                // Here we need to support base event subscribtion:
                // If event A is raised, handlers subscribing to A and A's base events all need to be invoked.
                var baseEventType = eventType.BaseType;

                while (baseEventType != null && baseEventType != typeof(object))
                {
                    handlerMethods.AddRange(FindDirectHandlers(baseEventType, holder));
                    baseEventType = baseEventType.BaseType;
                }

                foreach (var baseEventInterface in eventType.GetInterfaces())
                {
                    if (typeof(IEvent).IsAssignableFrom(baseEventInterface))
                    {
                        handlerMethods.AddRange(FindDirectHandlers(baseEventInterface, holder));
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return handlerMethods;
        }

        private IEnumerable<MethodInfo> FindDirectHandlers(Type eventType, Dictionary<Type, List<MethodInfo>> holder)
        {
            List<MethodInfo> handlerTypes = null;

            if (holder.TryGetValue(eventType, out handlerTypes))
            {
                return handlerTypes;
            }

            return Enumerable.Empty<MethodInfo>();
        }

        #endregion

        #region Registration

        public void RegisterAssemblies(params Assembly[] assemblies)
        {
            RegisterAssemblies(assemblies as IEnumerable<Assembly>);
        }

        public void RegisterAssemblies(IEnumerable<Assembly> assemblies)
        {
            _lock.EnterWriteLock();

            try
            {
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes();
                    RegisterEvents(types);
                    RegisterHandlers(types);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        #region Event Registration

        private void RegisterEvents(IEnumerable<Type> types)
        {
            var eventInterfaceType = typeof(IEvent);
            var eventTypes = types.Where(x => x.IsClass
                                            && x.IsPublic
                                            && !x.IsAbstract
                                            && eventInterfaceType.IsAssignableFrom(x)).ToList();

            if (eventTypes.Count == 0)
            {
                return;
            }

            foreach (var eventType in eventTypes)
            {
                foreach (var category in GetEventCategories(eventType))
                {
                    RegisterEvent(eventType, category);
                }
            }
        }

        private void RegisterEvent(Type eventType, string category)
        {
            List<Type> eventTypes = null;

            if (!_eventTypesByCategory.TryGetValue(category, out eventTypes))
            {
                eventTypes = new List<Type>();
                _eventTypesByCategory.Add(category, eventTypes);
            }

            eventTypes.Add(eventType);
        }

        private IEnumerable<string> GetEventCategories(Type eventType)
        {
            var categoryAttrs = eventType.GetCustomAttributes(typeof(EventCategoryAttribute), true)
                                         .OfType<EventCategoryAttribute>();

            var categories = new HashSet<string>(categoryAttrs.Select(x => x.Name));

            if (categories.Count == 0)
            {
                foreach (var interfaceType in eventType.GetInterfaces())
                {
                    var attrs = interfaceType.GetCustomAttributes(typeof(EventCategoryAttribute), true)
                                             .OfType<EventCategoryAttribute>();
                    foreach (var attr in attrs)
                    {
                        categories.Add(attr.Name);
                    }
                }
            }

            return categories;
        }

        #endregion

        #region Handler Registration

        private void RegisterHandlers(IEnumerable<Type> handlerTypes)
        {
            foreach (var type in handlerTypes)
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    RegisterHandler(type);
                }
            }
        }

        private bool RegisterHandler(Type handlerType)
        {
            Require.NotNull(handlerType, "handlerType");

            if (!handlerType.IsClass || handlerType.IsAbstract)
            {
                return false;
            }

            var eventTypes = EventHandlerUtil.GetHandledEventTypes(handlerType).ToList();

            if (eventTypes.Count == 0)
            {
                return false;
            }

            var thisHandlerMethods = handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                .Where(m => m.Name == "Handle" && m.ReturnType == typeof(void))
                                                .ToList();

            foreach (var eventType in eventTypes)
            {
                List<MethodInfo> handlerMethods = null;

                if (!_handlerMethodsByEventType.TryGetValue(eventType, out handlerMethods))
                {
                    handlerMethods = new List<MethodInfo>();
                    _handlerMethodsByEventType.Add(eventType, handlerMethods);
                }

                foreach (var method in thisHandlerMethods)
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length == 2 
                        && parameters[0].ParameterType == eventType
                        && parameters[1].ParameterType == typeof(EventDispatchingContext))
                    {
                        handlerMethods.Add(method);
                        break;
                    }
                }
            }

            return true;
        }

        #endregion

        #endregion

        public void Clear()
        {
            _lock.EnterWriteLock();

            try
            {
                _uncategorizedEvents.Clear();
                _eventTypesByCategory.Clear();
                _handlerMethodsByEventType.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
