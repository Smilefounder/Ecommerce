using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Categories;
using Kooboo.Commerce.Multilingual.Storage;
using System.Globalization;

namespace Kooboo.Commerce.Multilingual.Integration.ChangeSubscription
{
    class CategoryChangeSubscription : IHandle<CategoryUpdated>, IHandle<CategoryDeleted>
    {
        private IServiceFactory _serviceFactory;
        private ILanguageStore _languageStore;
        private ITranslationStore _translationStore;

        public CategoryChangeSubscription(IServiceFactory serviceFactory, ILanguageStore languageStore, ITranslationStore translationStore)
        {
            _serviceFactory = serviceFactory;
            _languageStore = languageStore;
            _translationStore = translationStore;
        }

        public void Handle(CategoryUpdated @event)
        {
            var key = new EntityKey(typeof(Category), @event.CategoryId);
            foreach (var lang in _languageStore.All())
            {
                _translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key);
            }
        }

        public void Handle(CategoryDeleted @event)
        {
            var key = new EntityKey(typeof(Category), @event.CategoryId);

            foreach (var lang in _languageStore.All())
            {
                _translationStore.Delete(CultureInfo.GetCultureInfo(lang.Name), key); 
            }
        }
    }
}