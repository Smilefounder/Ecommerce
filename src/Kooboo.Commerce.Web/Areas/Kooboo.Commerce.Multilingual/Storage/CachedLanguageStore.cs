using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage
{
    public class CachedLanguageStore : ILanguageStore
    {
        private ILanguageStore _underlyingStore;
        private Lazy<Dictionary<string, Language>> _cache;

        public CachedLanguageStore(ILanguageStore underlyingStore)
        {
            _underlyingStore = underlyingStore;
            ReinitializeCache();
        }

        private void ReinitializeCache()
        {
            _cache = new Lazy<Dictionary<string, Language>>(() =>
            {
                return _underlyingStore.All().ToDictionary(it => it.Name);
            }, true);
        }

        public IEnumerable<Language> All()
        {
            return _cache.Value.Values.Select(v => v.Clone()).ToList();
        }

        public bool Exists(string name)
        {
            return _cache.Value.ContainsKey(name);
        }

        public Language Find(string name)
        {
            Language language;
            if (_cache.Value.TryGetValue(name, out language))
            {
                return language.Clone();
            }

            return null;
        }

        public void Add(Language language)
        {
            _underlyingStore.Add(language);
            ReinitializeCache();
        }

        public void Update(Language language)
        {
            _underlyingStore.Update(language);
            ReinitializeCache();
        }

        public void Delete(string name)
        {
            _underlyingStore.Delete(name);
            ReinitializeCache();
        }
    }
}