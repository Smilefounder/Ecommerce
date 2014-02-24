using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public interface ICommerceDbProviderFactory
    {
        IEnumerable<ICommerceDbProvider> Providers { get; }

        ICommerceDbProvider GetDbProvider(string providerInvariantName, string providerManifestToken);
    }

    [Dependency(typeof(ICommerceDbProviderFactory), ComponentLifeStyle.Singleton)]
    public class CommerceDbProviderFactory : ICommerceDbProviderFactory
    {
        private readonly IEngine _engine;
        private readonly Lazy<Dictionary<CommerceDbProviderInfo, ICommerceDbProvider>> _providers;

        public CommerceDbProviderFactory()
            : this(EngineContext.Current)
        {
        }

        public CommerceDbProviderFactory(IEngine engine)
        {
            Require.NotNull(engine, "engine");
            _engine = engine;
            _providers = new Lazy<Dictionary<CommerceDbProviderInfo, ICommerceDbProvider>>(LoadProviders, true);
        }

        public IEnumerable<ICommerceDbProvider> Providers
        {
            get
            {
                return _providers.Value.Values;
            }
        }

        public ICommerceDbProvider GetDbProvider(string providerInvariantName, string providerManifestToken)
        {
            ICommerceDbProvider provider = null;

            if (_providers.Value.TryGetValue(new CommerceDbProviderInfo(providerInvariantName, providerManifestToken), out provider))
            {
                return provider;
            }

            return null;
        }

        private Dictionary<CommerceDbProviderInfo, ICommerceDbProvider> LoadProviders()
        {
            var providers = new Dictionary<CommerceDbProviderInfo, ICommerceDbProvider>();

            foreach (var provider in _engine.ResolveAll<ICommerceDbProvider>())
            {
                providers.Add(new CommerceDbProviderInfo(provider.InvariantName, provider.ManifestToken), provider);
            }

            return providers;
        }
    }
}
