using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Brands;
using Kooboo.Commerce.Multilingual.Storage;
using System.Globalization;

namespace Kooboo.Commerce.Multilingual.Integration.ChangeSubscription
{
    class BrandChangeSubscription : IHandle<BrandUpdated>, IHandle<BrandDeleted>
    {
        private ILanguageStore _languageStore;
        private ITranslationStore _translationStore;

        public BrandChangeSubscription()
        {
            _languageStore = LanguageStores.Get(CommerceInstance.Current.Name);
            _translationStore = TranslationStores.Get(CommerceInstance.Current.Name);
        }

        public void Handle(BrandUpdated @event)
        {
            var key = new EntityKey(typeof(Brand), @event.BrandId);
            foreach (var lang in _languageStore.All())
            {
                _translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }

        public void Handle(BrandDeleted @event)
        {
            var key = new EntityKey(typeof(Brand), @event.BrandId);

            foreach (var lang in _languageStore.All())
            {
                _translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }
    }
}