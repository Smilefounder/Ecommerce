﻿using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Multilingual.Events;
using Kooboo.Commerce.Multilingual.Storage;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Search.Models;
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
        private ILanguageStore _languageStore;

        public ProductEventsSubscription()
        {
            _languageStore = LanguageStores.Get(CommerceInstance.Current.Name);
        }

        #region Language Events

        public void Handle(LanguageAdded @event, CommerceInstance instance)
        {

        }

        public void Handle(LanguageDeleted @event, CommerceInstance instance)
        {

        }

        #endregion

        #region Translation Events

        public void Handle(TranslationUpdated @event, CommerceInstance instance)
        {
            var service = new ProductService(instance);

            if (@event.EntityKey.EntityType == typeof(Product))
            {
                var product = service.Find((int)@event.EntityKey.Value);
                if (product.IsPublished)
                {
                    Index(product, new[] { @event.Culture });
                }
            }
            else if (@event.EntityKey.EntityType == typeof(ProductVariant))
            {
                var variant = service.FindVariant((int)@event.EntityKey.Value);
                var product = service.Find(variant.ProductId);
                if (product.IsPublished)
                {
                    Index(product, new[] { @event.Culture });
                }
            }
        }

        #endregion

        #region Product Events

        public void Handle(ProductCreated @event, CommerceInstance instance)
        {
            var product = CommerceInstance.Current.Database.Repository<Product>().Find(@event.ProductId);
            if (product.IsPublished)
            {
                Index(product, GetAllCultures());
            }
        }

        public void Handle(ProductUpdated @event, CommerceInstance instance)
        {
            var product = CommerceInstance.Current.Database.Repository<Product>().Find(@event.ProductId);
            if (product.IsPublished)
            {
                Index(product, GetAllCultures());
            }
        }

        public void Handle(ProductPublished @event, CommerceInstance instance)
        {
            var product = CommerceInstance.Current.Database.Repository<Product>().Find(@event.ProductId);
            Index(product, GetAllCultures());
        }

        public void Handle(ProductUnpublished @event, CommerceInstance instance)
        {
            DeleteIndex(@event.ProductId);
        }

        public void Handle(ProductDeleted @event, CommerceInstance instance)
        {
            DeleteIndex(@event.ProductId);
        }

        #endregion

        private void Index(Product product, IEnumerable<CultureInfo> cultures)
        {
            foreach (var culture in cultures)
            {
                var indexer = IndexStores.Get<ProductModel>(CommerceInstance.Current.Name, culture);
                indexer.Index(ProductModel.Create(product, culture, CategoryTree.Get(CommerceInstance.Current.Name)));
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
            var indexer = IndexStores.Get<ProductModel>(CommerceInstance.Current.Name, CultureInfo.InvariantCulture);
            indexer.Delete(productId);
            indexer.Commit();

            foreach (var lang in _languageStore.All())
            {
                var langIndexer = IndexStores.Get<ProductModel>(CommerceInstance.Current.Name, CultureInfo.GetCultureInfo(lang.Name));
                langIndexer.Delete(productId);
                langIndexer.Commit();
            }
        }
    }
}