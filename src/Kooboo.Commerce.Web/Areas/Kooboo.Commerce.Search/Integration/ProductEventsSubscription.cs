using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Multilingual.Events;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Search.Builders;
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Integration
{
    class ProductEventsSubscription : 
        IHandle<ProductCreated>, IHandle<ProductUpdated>, IHandle<ProductPublished>, IHandle<ProductUnpublished>, IHandle<ProductDeleted>
        , IHandle<TranslationUpdated>
        , IHandle<LanguageAdded>, IHandle<LanguageDeleted>
    {
        private IServiceFactory _serviceFactory;
        private ILanguageStore _languageStore;

        public ProductEventsSubscription(IServiceFactory serviceFactory, ILanguageStore languageStore)
        {
            _serviceFactory = serviceFactory;
            _languageStore = languageStore;
        }

        #region Language Events

        public void Handle(LanguageAdded @event)
        {

        }

        public void Handle(LanguageDeleted @event)
        {

        }

        #endregion

        #region Translation Events

        public void Handle(TranslationUpdated @event)
        {
            if (@event.EntityKey.EntityType == typeof(Product))
            {
                IndexProductIfPublished((int)@event.EntityKey.Value);
            }
            else if (@event.EntityKey.EntityType == typeof(ProductVariant))
            {
                var variant = _serviceFactory.Products.GetProductVariantById((int)@event.EntityKey.Value);
                IndexProductIfPublished(variant.ProductId);
            }
        }

        #endregion

        #region Product Events

        public void Handle(ProductCreated @event)
        {
            IndexProductIfPublished(@event.ProductId);
        }

        public void Handle(ProductUpdated @event)
        {
            IndexProductIfPublished(@event.ProductId);
        }

        public void Handle(ProductPublished @event)
        {
            IndexProductIfPublished(@event.ProductId);
        }

        public void Handle(ProductUnpublished @event)
        {
            DeleteIndex(@event.ProductId);
        }

        public void Handle(ProductDeleted @event)
        {
            DeleteIndex(@event.ProductId);
        }

        #endregion

        private void IndexProductIfPublished(int productId)
        {
            var product = _serviceFactory.Products.GetById(productId);
            if (product.IsPublished)
            {
                var culture = CultureInfo.InvariantCulture;
                var productType = _serviceFactory.ProductTypes.GetById(product.ProductTypeId);

                var indexer = DocumentIndexers.GetIndexer(CommerceInstance.Current.Name, culture, typeof(Product));
                indexer.Index(ProductDocumentBuilder.Build(product, productType, culture));
                indexer.Commit();
            }
        }

        private void DeleteIndex(int productId)
        {
            var indexer = DocumentIndexers.GetIndexer(CommerceInstance.Current.Name, CultureInfo.InvariantCulture, typeof(Product));
            indexer.Delete(productId);
            indexer.Commit();

            foreach (var lang in _languageStore.All())
            {
                var langIndexer = DocumentIndexers.GetIndexer(CommerceInstance.Current.Name, CultureInfo.GetCultureInfo(lang.Name), typeof(Product));
                langIndexer.Delete(productId);
            }
        }
    }
}