using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.ProductTypes;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using System.Globalization;

namespace Kooboo.Commerce.Multilingual.Integration.ChangeSubscribtion
{
    class ProductTypeChangeSubscription : IHandle<ProductTypeUpdated>, IHandle<ProductTypeDeleted>
    {
        private IServiceFactory _serviceFactory;
        private ILanguageStore _languageStore;
        private ITranslationStore _translationStore;

        public ProductTypeChangeSubscription(IServiceFactory serviceFactory, ILanguageStore languageStore, ITranslationStore translationStore)
        {
            _serviceFactory = serviceFactory;
            _languageStore = languageStore;
            _translationStore = translationStore;
        }

        public void Handle(ProductTypeUpdated @event)
        {
            var key = new EntityKey(typeof(ProductType), @event.ProductTypeId);
            foreach (var lang in _languageStore.All())
            {
                _translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }

        public void Handle(ProductTypeDeleted @event)
        {
            var key = new EntityKey(typeof(ProductType), @event.ProductTypeId);

            foreach (var lang in _languageStore.All())
            {
                _translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }
    }
}