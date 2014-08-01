using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using System.Globalization;

namespace Kooboo.Commerce.Multilingual.Integration.ChangeSubscribtion
{
    class ProductVariantChangeSubscription : IHandle<ProductVariantCreated>, IHandle<ProductVariantUpdated>, IHandle<ProductVariantDeleted>
    {
        private ILanguageStore _languageStore;
        private ITranslationStore _translationStore;

        public ProductVariantChangeSubscription(ILanguageStore languageStore, ITranslationStore translationStore)
        {
            _languageStore = languageStore;
            _translationStore = translationStore;
        }

        public void Handle(ProductVariantCreated @event)
        {
            var productKey = new EntityKey(typeof(Product), @event.ProductId);
            foreach (var lang in _languageStore.All())
            {
                _translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), productKey);
            }
        }

        public void Handle(ProductVariantUpdated @event)
        {
            var productKey = new EntityKey(typeof(Product), @event.ProductId);
            foreach (var lang in _languageStore.All())
            {
                var culture = CultureInfo.GetCultureInfo(lang.Name);
                _translationStore.MarkOutOfDate(culture, new EntityKey(typeof(Product), productKey));
            }
        }

        public void Handle(ProductVariantDeleted @event)
        {
            var key = new EntityKey(typeof(ProductVariant), @event.ProductVariantId);
            foreach (var lang in _languageStore.All())
            {
                _translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }
    }
}