using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage
{
    public class CachedTranslactionStore : ITranslationStore
    {
        private ITranslationStore _underlyingStore;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private Dictionary<CultureInfo, Dictionary<EntityKey, EntityTransaltion>> _cache = new Dictionary<CultureInfo, Dictionary<EntityKey, EntityTransaltion>>();
        private HashSet<Type> _typesToCache;

        public CachedTranslactionStore(ITranslationStore underlyingStore)
            : this(underlyingStore, new[] { typeof(Brand), typeof(Category), typeof(ProductType) })
        {
        }

        public CachedTranslactionStore(ITranslationStore underlyingStore, IEnumerable<Type> typesToCache)
        {
            _underlyingStore = underlyingStore;
            _typesToCache = new HashSet<Type>(typesToCache);
        }

        public EntityTransaltion Find(System.Globalization.CultureInfo culture, EntityKey key)
        {
            return Find(culture, new EntityKey[] { key })[0];
        }

        public EntityTransaltion[] Find(System.Globalization.CultureInfo culture, params EntityKey[] keys)
        {
            var result = new EntityTransaltion[keys.Length];
            // Key: entity key, Value: the index in the result array
            var uncachedKeys = new List<KeyValuePair<EntityKey, int>>();

            _lock.EnterReadLock();

            try
            {
                if (_cache.ContainsKey(culture))
                {
                    var cache = _cache[culture];

                    for (var i = 0; i < keys.Length; i++)
                    {
                        EntityTransaltion translation;
                        if (cache.TryGetValue(keys[i], out translation))
                        {
                            result[i] = translation == null ? null : translation.Clone();
                        }
                        else
                        {
                            uncachedKeys.Add(new KeyValuePair<EntityKey, int>(keys[i], i));
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < keys.Length; i++)
                    {
                        uncachedKeys.Add(new KeyValuePair<EntityKey, int>(keys[i], i));
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

            // 对于不在缓存中的Key，要从底下的真实存储中查询
            if (uncachedKeys.Count > 0)
            {
                var translations = FindInUnderlyingStore(culture, uncachedKeys.Select(it => it.Key).ToArray());
                for (var i = 0; i < translations.Length; i++)
                {
                    var translation = translations[i];
                    var resultIndex = uncachedKeys[i].Value;
                    result[resultIndex] = IsEmptyTranslation(translation) ? null : translation.Clone();
                }

                UpdateCache(culture, translations);
            }

            return result;
        }

        private EntityTransaltion[] FindInUnderlyingStore(CultureInfo culture, EntityKey[] keys)
        {
            var translations = _underlyingStore.Find(culture, keys);
            for (var i = 0; i < translations.Length; i++)
            {
                // 保证返回的结果不为null, 这样在更新缓存时可以得到EntityKey和Culture信息，以便缓存“未翻译”结果
                if (translations[i] == null)
                {
                    translations[i] = new EmptyEntityTranslation(culture.Name, keys[i]);
                }
            }

            return translations;
        }

        public int TotalTranslated(System.Globalization.CultureInfo culture, Type entityType)
        {
            return _underlyingStore.TotalTranslated(culture, entityType);
        }

        public int TotalOutOfDate(System.Globalization.CultureInfo culture, Type entityType)
        {
            return _underlyingStore.TotalOutOfDate(culture, entityType);
        }

        public Pagination<EntityTransaltion> FindOutOfDate(System.Globalization.CultureInfo culture, Type entityType, int pageIndex, int pageSize)
        {
            return _underlyingStore.FindOutOfDate(culture, entityType, pageIndex, pageSize);
        }

        public void AddOrUpdate(System.Globalization.CultureInfo culture, EntityKey key, IEnumerable<PropertyTranslation> propertyTranslations)
        {
            _underlyingStore.AddOrUpdate(culture, key, propertyTranslations);
            UpdateCache(culture, new List<EntityTransaltion> { new EntityTransaltion(culture.Name, key, propertyTranslations) });
        }

        public void MarkOutOfDate(System.Globalization.CultureInfo culture, EntityKey key)
        {
            _underlyingStore.MarkOutOfDate(culture, key);
            RemoveCache(culture, key);
        }

        public void Delete(System.Globalization.CultureInfo culture, EntityKey key)
        {
            _underlyingStore.Delete(culture, key);
            RemoveCache(culture, key);
        }

        private bool NeedToBeCached(Type entityType)
        {
            return _typesToCache.Contains(entityType);
        }

        private void UpdateCache(CultureInfo culture, IEnumerable<EntityTransaltion> translations)
        {
            // 只有指定的类型才需要缓存
            var translationsNeedToBeCached = translations.Where(t => NeedToBeCached(t.EntityKey.EntityType)).ToList();
            if (translationsNeedToBeCached.Count == 0)
            {
                return;
            }

            _lock.EnterWriteLock();

            try
            {
                if (!_cache.ContainsKey(culture))
                {
                    _cache.Add(culture, new Dictionary<EntityKey, EntityTransaltion>());
                }

                var cache = _cache[culture];

                foreach (var translation in translationsNeedToBeCached)
                {
                    var cacheItem = IsEmptyTranslation(translation) ? null : translation.Clone();

                    if (cache.ContainsKey(translation.EntityKey))
                    {
                        cache[translation.EntityKey] = cacheItem;
                    }
                    else
                    {
                        cache.Add(translation.EntityKey, cacheItem);
                    }
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private void RemoveCache(CultureInfo culture, EntityKey entityKey)
        {
            _lock.EnterWriteLock();

            try
            {
                if (_cache.ContainsKey(culture))
                {
                    _cache[culture].Remove(entityKey);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private bool IsEmptyTranslation(EntityTransaltion translation)
        {
            return translation == null || translation is EmptyEntityTranslation;
        }

        public class EmptyEntityTranslation : EntityTransaltion
        {
            public EmptyEntityTranslation(string culture, EntityKey entityKey)
                : base(culture, entityKey)
            {
            }
        }
    }
}