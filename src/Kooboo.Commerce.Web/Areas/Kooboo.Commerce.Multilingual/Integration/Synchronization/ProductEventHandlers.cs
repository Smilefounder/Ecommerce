using System;
using System.Linq;
using System.Globalization;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Multilingual.Integration.Synchronization
{
    class ProductEventHandlers : IHandle<ProductUpdated>, IHandle<ProductDeleted>
    {
        public Func<string, ILanguageStore> GetLanguageStoreByInstance = instance => LanguageStores.Get(instance);

        public Func<string, ITranslationStore> GetTranlsationStoreByInstance = instance => TranslationStores.Get(instance);

        public void Handle(ProductUpdated @event, EventContext context)
        {
            var instance = context.Instance;
            var languageStore = GetLanguageStoreByInstance(instance.Name);
            var translationStore = GetTranlsationStoreByInstance(instance.Name);
            
            var key = new EntityKey(typeof(Product), @event.ProductId);

            foreach (var lang in languageStore.All())
            {
                translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }

        public void Handle(ProductDeleted @event, EventContext context)
        {
            var instance = context.Instance;
            var languageStore = GetLanguageStoreByInstance(instance.Name);
            var translationStore = GetTranlsationStoreByInstance(instance.Name);
            
            var key = new EntityKey(typeof(Product), @event.ProductId);

            foreach (var lang in languageStore.All())
            {
                translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }
    }
}