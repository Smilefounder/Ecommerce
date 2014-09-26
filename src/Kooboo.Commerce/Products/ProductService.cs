using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;

namespace Kooboo.Commerce.Products
{
    [Dependency(typeof(ProductService))]
    public class ProductService
    {
        private readonly CommerceInstance _instance;
        private readonly ICommerceDatabase _database;

        public ProductService(CommerceInstance instance)
        {
            _instance = instance;
            _database = instance.Database;
        }

        public Product Find(int id)
        {
            return _database.Repository<Product>().Find(id);
        }

        public IQueryable<Product> Query()
        {
            return _database.Repository<Product>().Query();
        }

        public IQueryable<ProductVariant> ProductVariants()
        {
            return _database.Repository<ProductVariant>().Query();
        }

        public ProductVariant FindVariant(int id)
        {
            return _database.Repository<ProductVariant>().Find(id);
        }

        public void Create(Product product)
        {
            SyncPriceRange(product);
            _database.Repository<Product>().Insert(product);
            Event.Raise(new ProductCreated(product), _instance);
        }

        public void Update(Product product)
        {
            SyncPriceRange(product);
            _database.Repository<Product>().Update(product);
            Event.Raise(new ProductUpdated(product), _instance);
        }

        private void SyncPriceRange(Product product)
        {
            if (product.Variants.Count == 0)
            {
                product.LowestPrice = product.HighestPrice = 0;
            }
            else
            {
                product.LowestPrice = product.Variants.Min(v => v.Price);
                product.HighestPrice = product.Variants.Max(v => v.Price);
            }
        }

        public void Delete(Product model)
        {
            var product = _database.Repository<Product>().Find(model.Id);
            _database.Repository<Product>().Delete(product);
            Event.Raise(new ProductDeleted(product), _instance);
        }

        public bool Publish(Product product)
        {
            if (product.IsPublished)
            {
                return false;
            }

            product.IsPublished = true;

            _database.SaveChanges();

            Event.Raise(new ProductPublished(product), _instance);

            return true;
        }

        public bool Unpublish(Product product)
        {
            if (!product.IsPublished)
            {
                return false;
            }

            product.IsPublished = false;

            _database.SaveChanges();

            Event.Raise(new ProductUnpublished(product), _instance);

            return true;
        }
    }
}