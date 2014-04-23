using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    [Dependency(typeof(CommerceInstanceContext), ComponentLifeStyle.InRequestScope)]
    public class CommerceInstanceContext : IDisposable
    {
        private ICommerceInstanceNameResolver _nameResolver;
        private ICommerceInstanceManager _instanceManager;

        private CommerceInstance _currentInstance;

        public CommerceInstance CurrentInstance
        {
            get
            {                   
                // TODO: We hack this for activity execution job for now!
                //       It might be better to refactor CommerceInstanceContext to Scope to provider more controllable object scope.
                var instance = Scope<CommerceInstance>.Current;
                if (instance != null)
                {
                    // Simply return without setting _currentInstance.
                    // Because scope could be disposed
                    return instance;
                }

                if (_currentInstance == null)
                {
                    var name = _nameResolver.GetCurrentInstanceName();
                    if (!String.IsNullOrWhiteSpace(name))
                    {
                        _currentInstance = _instanceManager.OpenInstance(name);
                    }
                }

                return _currentInstance;
            }
        }

        public CommerceInstanceContext(
            ICommerceInstanceNameResolver nameResolver,
            ICommerceInstanceManager instanceManager)
        {
            Require.NotNull(nameResolver, "nameResolver");
            Require.NotNull(instanceManager, "instanceManager");

            _instanceManager = instanceManager;
            _nameResolver = nameResolver;
        }

        public void Dispose()
        {
            if (_currentInstance != null)
            {
                _currentInstance.Dispose();
            }
        }
    }
}
