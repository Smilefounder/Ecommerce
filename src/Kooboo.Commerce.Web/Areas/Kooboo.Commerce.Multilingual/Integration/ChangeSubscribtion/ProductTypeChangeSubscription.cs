using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.ProductTypes;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using System.Globalization;

namespace Kooboo.Commerce.Multilingual.Integration.ChangeSubscribtion
{
    class ProductTypeChangeSubscription : IHandle<ProductTypeUpdated>, IHandle<ProductTypeDeleted>
    {
        private ILanguageStore _languageStore;
        private ITranslationStore _translationStore;

        public ProductTypeChangeSubscription()
        {
            _languageStore = LanguageStores.Get(CommerceInstance.Current.Name);
            _translationStore = TranslationStores.Get(CommerceInstance.Current.Name);
        }

        public void Handle(ProductTypeUpdated @event, CommerceInstance instance)
        {
            var key = new EntityKey(typeof(ProductType), @event.ProductTypeId);
            foreach (var lang in _languageStore.All())
            {
                _translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }

        public void Handle(ProductTypeDeleted @event, CommerceInstance instance)
        {
            var key = new EntityKey(typeof(ProductType), @event.ProductTypeId);

            foreach (var lang in _languageStore.All())
            {
                _translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }
    }
}