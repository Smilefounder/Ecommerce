using System;
using System.Globalization;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Categories;
using Kooboo.Commerce.Multilingual.Storage;

namespace Kooboo.Commerce.Multilingual.Integration.Synchronization
{
    class CategoryEventHandlers : IHandle<CategoryUpdated>, IHandle<CategoryDeleted>
    {
        public Func<string, ILanguageStore> GetLanguageStoreByInstance = instance => LanguageStores.Get(instance);

        public Func<string, ITranslationStore> GetTranlsationStoreByInstance = instance => TranslationStores.Get(instance);

        public void Handle(CategoryUpdated @event, CommerceInstance instance)
        {
            var languageStore = GetLanguageStoreByInstance(instance.Name);
            var translationStore = GetTranlsationStoreByInstance(instance.Name);

            var key = new EntityKey(typeof(Category), @event.CategoryId);
            foreach (var lang in languageStore.All())
            {
                translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }

        public void Handle(CategoryDeleted @event, CommerceInstance instance)
        {
            var languageStore = GetLanguageStoreByInstance(instance.Name);
            var translationStore = GetTranlsationStoreByInstance(instance.Name);

            var key = new EntityKey(typeof(Category), @event.CategoryId);

            foreach (var lang in languageStore.All())
            {
                translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key); 
            }
        }
    }
}