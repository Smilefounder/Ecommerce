using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class ParameterProviderManager
    {
        readonly ConcurrentBag<IParameterProvider> _providers = new ConcurrentBag<IParameterProvider>();

        public IEnumerable<IParameterProvider> Providers
        {
            get
            {
                return _providers.ToList();
            }
        }

        public void Add(IParameterProvider provider)
        {
            Require.NotNull(provider, "provider");

            _providers.Add(provider);
        }

        public static readonly ParameterProviderManager Instance = new ParameterProviderManager();

        static ParameterProviderManager()
        {
            Instance.Add(new DeclaringParameterProvider());
        }
    }
}
