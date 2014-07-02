using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Providers
{
    public class CommerceDbProviderCollection : IEnumerable<ICommerceDbProvider>
    {
        readonly Dictionary<CommerceDbProviderInfo, ICommerceDbProvider> _providers = new Dictionary<CommerceDbProviderInfo, ICommerceDbProvider>();

        public int Count
        {
            get
            {
                return _providers.Count;
            }
        }

        public void Add(ICommerceDbProvider provider)
        {
            Require.NotNull(provider, "provider");

            var key = new CommerceDbProviderInfo(provider.InvariantName, provider.ManifestToken);
            if (_providers.ContainsKey(key))
                throw new InvalidOperationException("Provider with the same invariant name and manifest token alredy exists.");

            _providers.Add(key, provider);
        }

        public ICommerceDbProvider Find(string invariantName, string manifestToken)
        {
            ICommerceDbProvider provider;

            if (_providers.TryGetValue(new CommerceDbProviderInfo(invariantName, manifestToken), out provider))
            {
                return provider;
            }

            return null;
        }

        public IEnumerator<ICommerceDbProvider> GetEnumerator()
        {
            return _providers.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
