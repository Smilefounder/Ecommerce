using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Brands;
using Kooboo.Commerce.Multilingual.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Integration.ChangeSubscription
{
    class BrandChangeSubscription : IHandle<BrandUpdated>, IHandle<BrandDeleted>
    {
        private ILanguageStore _languageStore;
        private ITranslationStore _translationStore;
        private IServiceFactory _serviceFactory;

        public BrandChangeSubscription(IServiceFactory serviceFactory, ILanguageStore languageStore, ITranslationStore translationStore)
        {
            _serviceFactory = serviceFactory;
            _languageStore = languageStore;
            _translationStore = translationStore;
        }

        public void Handle(BrandUpdated @event)
        {
            var brand = _serviceFactory.Brands.GetById(@event.BrandId);
            var key = new EntityKey(typeof(Brand), brand.Id);
            var updates = new Dictionary<string, string>
            {
                { "Name", brand.Name },
                { "Description", brand.Description }
            };

            foreach (var lang in _languageStore.All())
            {
                _translationStore.MarkOutOfDate(CultureInfo.GetCultureInfo(lang.Name), key, updates);
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