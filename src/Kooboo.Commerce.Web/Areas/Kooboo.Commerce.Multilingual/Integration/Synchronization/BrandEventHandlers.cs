using System;
using System.Globalization;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Brands;
using Kooboo.Commerce.Multilingual.Storage;

namespace Kooboo.Commerce.Multilingual.Integration.Synchronization
{
    class BrandEventHandlers : IHandle<BrandUpdated>, IHandle<BrandDeleted>
    {
        public Func<string, ILanguageStore> GetLanguageStoreByInstance = instance => LanguageStores.Get(instance);

        public Func<string, ITranslationStore> GetTranlsationStoreByInstance = instance => TranslationStores.Get(instance);

        public void Handle(BrandUpdated @event, CommerceInstance instance)
        {
            var languageStore = GetLanguageStoreByInstance(instance.Name);
            var translationStore = GetTranlsationStoreByInstance(instance.Name);

            var key = new EntityKey(typeof(Brand), @event.BrandId);
            foreach (var lang in languageStore.All())
            {
                translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }

        public void Handle(BrandDeleted @event, CommerceInstance instance)
        {
            var languageStore = GetLanguageStoreByInstance(instance.Name);
            var translationStore = GetTranlsationStoreByInstance(instance.Name);

            var key = new EntityKey(typeof(Brand), @event.BrandId);

            foreach (var lang in languageStore.All())
            {
                translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }
    }
}