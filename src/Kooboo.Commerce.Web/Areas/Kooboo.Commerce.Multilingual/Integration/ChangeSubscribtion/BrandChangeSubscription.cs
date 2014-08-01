using Kooboo.Commerce.Brands;
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

        public BrandChangeSubscription(ILanguageStore languageStore, ITranslationStore translationStore)
        {
            _languageStore = languageStore;
            _translationStore = translationStore;
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