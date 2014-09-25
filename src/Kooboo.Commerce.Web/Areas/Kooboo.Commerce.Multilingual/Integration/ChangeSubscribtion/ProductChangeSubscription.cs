using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using System.Globalization;
using System.Linq;

namespace Kooboo.Commerce.Multilingual.Integration.ChangeSubscription
{
    class ProductChangeSubscription : IHandle<ProductUpdated>, IHandle<ProductDeleted>
    {
        private ILanguageStore _languageStore;
        private ITranslationStore _translationStore;

        public ProductChangeSubscription()
        {
            _languageStore = LanguageStores.Get(CommerceInstance.Current.Name);
            _translationStore = TranslationStores.Get(CommerceInstance.Current.Name);
        }

        public void Handle(ProductUpdated @event, CommerceInstance instance)
        {
            var key = new EntityKey(typeof(Product), @event.ProductId);
            var languages = _languageStore.All().ToList();
            foreach (var lang in languages)
            {
                _translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }

        public void Handle(ProductDeleted @event, CommerceInstance instance)
        {
            var key = new EntityKey(typeof(Product), @event.ProductId);

            foreach (var lang in _languageStore.All())
            {
                _translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }
    }
}