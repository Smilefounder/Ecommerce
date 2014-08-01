using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Categories;
using Kooboo.Commerce.Multilingual.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

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
            var category = _serviceFactory.Categories.GetById(@event.CategoryId);
            var key = new EntityKey(typeof(Category), category.Id);
            var updates = new Dictionary<string, string>
            {
                { "Name", category.Name },
                { "Description", category.Description }
            };

            foreach (var lang in _languageStore.All())
            {
                _translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key, updates);
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