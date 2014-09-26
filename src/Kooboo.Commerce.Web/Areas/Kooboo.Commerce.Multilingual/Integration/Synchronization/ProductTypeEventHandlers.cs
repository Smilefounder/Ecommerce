using System;
using System.Globalization;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.ProductTypes;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Multilingual.Integration.ChangeSubscribtion
{
    class ProductTypeEventHandlers : IHandle<ProductTypeUpdated>, IHandle<ProductTypeDeleted>
    {
        public Func<string, ILanguageStore> GetLanguageStoreByInstance = instance => LanguageStores.Get(instance);

        public Func<string, ITranslationStore> GetTranlsationStoreByInstance = instance => TranslationStores.Get(instance);

        public void Handle(ProductTypeUpdated @event, CommerceInstance instance)
        {
            var languageStore = GetLanguageStoreByInstance(instance.Name);
            var translationStore = GetTranlsationStoreByInstance(instance.Name);

            var key = new EntityKey(typeof(ProductType), @event.ProductTypeId);
            foreach (var lang in languageStore.All())
            {
                translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }

        public void Handle(ProductTypeDeleted @event, CommerceInstance instance)
        {
            var languageStore = GetLanguageStoreByInstance(instance.Name);
            var translationStore = GetTranlsationStoreByInstance(instance.Name);
            
            var key = new EntityKey(typeof(ProductType), @event.ProductTypeId);

            foreach (var lang in languageStore.All())
            {
                translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }
    }
}