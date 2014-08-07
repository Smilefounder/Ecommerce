﻿using Kooboo.Commerce.Data;
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

namespace Kooboo.Commerce.Search.Subscriptions
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
                var product = _serviceFactory.Products.GetById((int)@event.EntityKey.Value);
                if (product.IsPublished)
                {
                    var productType = _serviceFactory.ProductTypes.GetById(product.ProductTypeId);
                    Index(product, productType, new[] { @event.Culture });
                }
            }
            else if (@event.EntityKey.EntityType == typeof(ProductVariant))
            {
                var variant = _serviceFactory.Products.GetProductVariantById((int)@event.EntityKey.Value);
                var product = _serviceFactory.Products.GetById(variant.ProductId);
                if (product.IsPublished)
                {
                    var productType = _serviceFactory.ProductTypes.GetById(product.ProductTypeId);
                    Index(product, productType, new[] { @event.Culture });
                }
            }
        }

        #endregion

        #region Product Events

        public void Handle(ProductCreated @event)
        {
            var product = _serviceFactory.Products.GetById(@event.ProductId);
            if (product.IsPublished)
            {
                var productType = _serviceFactory.ProductTypes.GetById(product.ProductTypeId);
                Index(product, productType, GetAllCultures());
            }
        }

        public void Handle(ProductUpdated @event)
        {
            var product = _serviceFactory.Products.GetById(@event.ProductId);
            if (product.IsPublished)
            {
                var productType = _serviceFactory.ProductTypes.GetById(product.ProductTypeId);
                Index(product, productType, GetAllCultures());
            }
        }

        public void Handle(ProductPublished @event)
        {
            var product = _serviceFactory.Products.GetById(@event.ProductId);
            var productType = _serviceFactory.ProductTypes.GetById(product.ProductTypeId);
            Index(product, productType, GetAllCultures());
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

        private void Index(Product product, ProductType productType, IEnumerable<CultureInfo> cultures)
        {
            foreach (var culture in cultures)
            {
                var indexer = DocumentIndexers.GetLiveIndexer(CommerceInstance.Current.Name, typeof(Product), culture);
                indexer.Index(ProductDocumentBuilder.Build(product, productType, culture));
                indexer.Commit();
            }
        }

        private IEnumerable<CultureInfo> GetAllCultures()
        {
            var cultures = new List<CultureInfo> { CultureInfo.InvariantCulture };
            foreach (var lang in _languageStore.All())
            {
                cultures.Add(CultureInfo.GetCultureInfo(lang.Name));
            }

            return cultures;
        }

        private void DeleteIndex(int productId)
        {
            var indexer = DocumentIndexers.GetLiveIndexer(CommerceInstance.Current.Name, typeof(Product), CultureInfo.InvariantCulture);
            indexer.Delete(productId);
            indexer.Commit();

            foreach (var lang in _languageStore.All())
            {
                var langIndexer = DocumentIndexers.GetLiveIndexer(CommerceInstance.Current.Name, typeof(Product), CultureInfo.GetCultureInfo(lang.Name));
                langIndexer.Delete(productId);
                langIndexer.Commit();
            }
        }
    }
}