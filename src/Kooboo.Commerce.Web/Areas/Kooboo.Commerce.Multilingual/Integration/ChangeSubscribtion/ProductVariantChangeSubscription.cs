using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Integration.ChangeSubscribtion
{
    class ProductVariantChangeSubscription : IHandle<ProductVariantCreated>, IHandle<ProductVariantUpdated>, IHandle<ProductVariantDeleted>
    {
        private IServiceFactory _serviceFactory;
        private ILanguageStore _languageStore;
        private ITranslationStore _translationStore;

        public ProductVariantChangeSubscription(IServiceFactory serviceFactory, ILanguageStore languageStore, ITranslationStore translationStore)
        {
            _serviceFactory = serviceFactory;
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
            var variant = _serviceFactory.Products.GetProductVariantById(@event.ProductVariantId);
            var key = new EntityKey(typeof(ProductVariant), variant.Id);
            var updates = new Dictionary<string, string>();
            foreach (var field in variant.VariantFields)
            {
                updates.Add(field.FieldName, field.FieldValue);
            }

            foreach (var lang in _languageStore.All())
            {
                var culture = CultureInfo.GetCultureInfo(lang.Name);
                var updated = _translationStore.MarkOutOfDate(culture, key, updates);
                if (updated)
                {
                    _translationStore.MarkOutOfDate(culture, new EntityKey(typeof(Product), variant.ProductId));
                }
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